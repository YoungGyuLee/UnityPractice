using System.Collections;
using System.Collections.Generic;
using Unity.Collections;
using UnityEngine;
using Unity.Mathematics;
using System.Runtime.InteropServices;
using Random = UnityEngine.Random;

public class MyScript : MonoBehaviour
{

    struct Particle
    {
        public float2 x;
        public float2 v;
        public float2x2 C;
        public float mass;
        public float padding;

    }

    struct Cell
    {
        public float2 v;
        public float mass;
        public float padding;
    }

    const int grid_res = 64;
    //구역을 정한 것 같다. 총 64개 구역에
    const int num_cells = grid_res * grid_res;
    //각 구역마다 셀이 64개. 셀은 파티클을 넣는 건가..?

    const float dt = 1.0f;
    //dt는 시뮬레이션 단위란다.

    const int iterations = (int)(1.0f / dt);
    //반복 횟수

    const float gravity = -0.05f;
    //중력인 듯.

    int num_partices;
    //파티클 수

    NativeArray<Particle> ps;
    //배열인데.. 단순한 배열은 아닌 듯함.
    NativeArray<Cell> grid;

    float2[] weights = new float2[3];

    SimRenderer sim_renderer;

    const float mouse_radius = 10;
    bool mouse_down = false;
    float2 mouse_pos;
    //위 셋은 interaction을 위한 변수들.


    // Start is called before the first frame update
    void Start()
    {
        List<float2> temp_positions = new List<float2>();
        const float spacing = 1.0f;
        //입자간 간격인가..?
        const int box_x = 16, box_y = 16;
        //박스 사이즈인 듯.
        const float sx = grid_res / 2.0f, sy = grid_res / 2.0f;
        //2.0은 어디서..?

        for(float i = sx - box_x / 2; i < sx + box_x / 2; i += spacing)
        {
            for(float j = sy - box_y/2; j<sy+box_y / 2; j += spacing)
            {
                var pos = math.float2(i, j);
                temp_positions.Add(pos);
            }
        }
        //particle들의 초기 위치를 잡는 듯?

        num_partices = temp_positions.Count;
        //위치를 파티클 수 만큼 만들었으니 이런 식으로 파티클 수 넘기는 것이 가능.

        ps = new NativeArray<Particle>(num_partices, Allocator.Persistent);
        //실제 파티클. 배열 생성 시 파티클 수와 어...성능을 좌우하는 생명주기..인가..?

        for(int i = 0; i< num_partices; i++)
        {
            Particle p = new Particle();
            p.x = temp_positions[i];
            //particle.x는 float2짜리.
            p.v = math.float2(Random.value - 0.5f, Random.value - 0.5f + 2.75f) * 0.5f;
            p.C = 0;
            p.mass = 1.0f;
            ps[i] = p;
            //파티클 배열에서 i번째 파티클에 위에서 만든 p를 넣어 줌. 그렇다는 건 위치가 랜덤이란 건가..?

        }

        grid = new NativeArray<Cell>(num_cells, Allocator.Persistent);

        for(int i = 0; i<num_cells; i++)
        {
            grid[i] = new Cell();
        }

        sim_renderer = GameObject.FindObjectOfType<SimRenderer>();
        sim_renderer.Initialise(num_partices, Marshal.SizeOf(new Particle()));
        //Marhsal은 뭐지..

        //아무튼 여기까지는 파티클들을 생성하고 위치를 임의로 생성하며 그 파티클들이 들어 갈 셀들을 정의하는 단계인 듯.
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    void HandleMouseInteraction()
    {
        mouse_down = false;
        if (Input.GetMouseButton(0))
        {
            mouse_down = true;
            var mp = Camera.main.ScreenToViewportPoint(Input.mousePosition);
            mouse_pos = math.float2(mp.x * grid_res, mp.y * grid_res);
        }
    }

    void Simulate()
    {
        for (int i = 0; i < num_cells; i++)
        {
            var cell = grid[i];
            //그리드는 셀 배열
            cell.mass = 0;
            cell.v = 0;

            grid[i] = cell;
        }
        //그리드 리셋하는 과정.

        for (int i = 0; i < num_partices; i++)
        {
            var p = ps[i];
            //ps에 있는 것 중 i번째를 할당

            uint2 cell_idx = (uint2)p.x;
            //int 2자리로 변환.
            float2 cell_diff = (p.x - cell_idx) - 0.5f;
            //파티클 위치와 셀 위치(셀 위치는 정수 표현)에다가 0.5를 빼서 보정은 아니고 뭐지..? 여튼 diff 구함.
            weights[0] = 0.5f * math.pow(0.5f - cell_diff, 2);
            weights[1] = 0.75f - math.pow(cell_diff, 2);
            weights[2] = 0.5f * math.pow(0.5f + cell_diff, 2);

            for (uint gx = 0; gx < 3; ++gx)
            {
                for(uint gy = 0; gy < 3; ++gy)
                {
                    float weight = weights[gx].x * weights[gy].y;

                    uint2 cell_x = math.uint2(cell_idx.x + gx - 1, cell_idx.y + gy - 1);
                    //cell_idx는 i번째 파티클의 x좌표의 정수값.
                    float2 cell_dist = (cell_x - p.x) + 0.5f;
                    float2 Q = math.mul(p.C, cell_dist);

                    float mass_contrib = weight * p.mass;
                    //여기가 MPM 방정식 172번..?

                    int cell_index = (int)cell_x.x * grid_res + (int)cell_x.y;
                    //grid_res는 그리드의 구역 수. cell_index는 각 cell의 index 계산법인 듯.
                    Cell cell = grid[cell_index];
                    //2d를 1d로 바꾸는 과정이란다.

                    cell.mass += mass_contrib;
                    //grid에 mass 뿌리기


                    cell.v += mass_contrib * (p.v + Q);
                    grid[cell_index] = cell;
                    //cell.v는 의 모멘텀(운동 에너지). 속도가 아님.

                }
            }
        }

        //여기서부터는 그리드 속도 업데이트
        for(int i = 0; i<num_cells; ++i)
        {
            var cell = grid[i];

            if(cell.mass > 0)
            {
                //여기서부터 모멘텀을 중력 적용하여 속도로 바꿀 예정
                cell.v /= cell.mass;
                cell.v += dt * math.float2(0, gravity);

                int x = i / grid_res;
                int y = i % grid_res;
                if(x < 2 || x > grid_res - 3)
                {
                    cell.v.x = 0;
                }
                if(y < 2 || y > grid_res - 3)
                {
                    cell.v.y = 0;
                }

                grid[i] = cell; 
            }
        }


        //여기서부터는 그리드를 파티클로 바꿀 거임.
        for(int i = 0; i<num_partices; ++i)
        {
            var p = ps[i];

            p.v = 0;
            //파티클 속도는 일단 0으로 초기화.

            //quadratic interpolation
            uint2 cell_idx = (uint2)p.x;
            float2 cell_diff = (p.x - cell_idx) - 0.5f;
            weights[0] = 0.5f * math.pow(0.5f - cell_diff, 2);
            weights[1] = 0.75f - math.pow(cell_diff, 2);
            weights[2] = 0.5f * math.pow(0.5f + cell_diff, 2);

            float2x2 B = 0;


            //아래는 APIC논문(https://web.archive.org/web/20190427165435/https://www.math.ucla.edu/~jteran/papers/JSSTS15.pdf), page 6에 의거해여
            //8번 식을 세운 것..?
            for (uint gx = 0; gx < 3; ++gx)
            {
                for(uint gy = 0; gy < 3; ++gy)
                {
                    float weight = weights[gx].x * weights[gy].y;

                    uint2 cell_x = math.uint2(cell_idx.x - 1, cell_idx.y + gy - 1);
                    int cell_index = (int)cell_x.x * grid_res + (int)cell_x.y;

                    float2 dist = (cell_x - p.x) + 0.5f;
                    float2 weighted_velocity = grid[cell_index].v * weight;

                    var term = math.float2x2(weighted_velocity * dist.x, weighted_velocity * dist.y);
                    //APIC paper 방정식 10번. B에 해당하는 것을 구함.
                    //아무래도 파티클이 있을 위치를 구하는 듯..?

                    B += term;

                    p.v += weighted_velocity;

                }
            }
            p.C = B * 4;

            p.x += p.v * dt;

            p.x = math.clamp(p.x, 1, grid_res - 2);
            //최대, 최소를 설정하여 그 이상 넘지 않도록 하는 함수.

            if (mouse_down)
            {
                var dist = p.x - mouse_pos;
                if(math.dot(dist, dist) < mouse_radius * mouse_radius)
                {//내적
                    float norm_factor = (math.length(dist) / mouse_radius);
                    norm_factor = math.pow(math.sqrt(norm_factor), 8);
                    var force = math.normalize(dist) * norm_factor * 0.5f;
                    p.v += force;
                }
            }
            //이건 마우스 움직였을 때 위치 조정.

            ps[i] = p;
            //i번째 ps에 위에서 조작하여 만들었던 p를 넣음.
            
        }
    }

    private void OnDestroy()
    {
        ps.Dispose();
        grid.Dispose();
    }
}
