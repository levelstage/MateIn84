using GfEngine.Visuals;
using GfEngine.Inputs;

namespace GfEngine.Systems;

// 게임 전체를 총괄하는 관리자 (엔진의 본체이자 모든 관리자의 상관)
public class SessionManager
{
    // 1. 싱글턴 인스턴스
    private static SessionManager? _instance;
    public static SessionManager Instance => _instance ??= new SessionManager();

    // 2. 핵심 의존성 (생성자가 아니라 Init으로 받음)
    public IInputAdapter? Input { get; private set; }  // Input Stream
    public IVisualizer? Visualizer { get; private set; }  // Output Stream

    private SessionManager() { } // 외부 생성 막음

    // 3. 시동 걸기 (클라이언트가 호출)
    public void Initialize(IVisualizer visualizer, IInputAdapter input)
    {
        Visualizer = visualizer;
        Input = input;
        Console.WriteLine("Engine Started. Ready to roll.");
    }

    // 4. 게임 시작
    public void StartNewGame()
    {
        // ... 세이브 데이터 로드 ...
        // ... 첫 장면(마을 or 배틀) 로드 ...
    }
}