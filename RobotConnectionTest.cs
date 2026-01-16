using System.Net.Sockets;
using System.Text;

namespace Login;

public static class RobotConnectionTest
{
    private const string RobotIp = "172.20.254.201";   // <-- brug den IP som VIRKEDE før
    private const int UrScriptPort = 30002;

    // Sender HELE scriptet i én TCP-forbindelse (vigtigt!)
    private static void SendProgram(string urScriptProgram)
    {
        using var client = new TcpClient(RobotIp, UrScriptPort);
        using var stream = client.GetStream();

        var bytes = Encoding.ASCII.GetBytes(urScriptProgram);
        stream.Write(bytes, 0, bytes.Length);
        stream.Flush();
    }

    // ✅ Sanity test: den som du siger virkede før
    public static void MoveToP2_Single()
    {
        SendProgram(
            "def prog():\n" +
            "  movej(p[0.482,-0.118,0.044,3.182,-0.003,-0.009], a=1.2, v=0.25)\n" +
            "end\n" +
            "prog()\n"
        );
    }

    public static void RunComponentA()
    {
        SendProgram(
            "def prog():\n" +
            "  p1 = p[0.130,-0.345,0.548,2.01,-0.001,-0.007]\n" +
            "  p2 = p[0.482,-0.118,0.044,3.182,-0.003,-0.009]\n" +
            "  p6 = p[0.482,-0.118,-0.125,3.182,-0.003,-0.009]\n" +
            "  p5 = p[0.027,-0.482,0.044,2.508,-1.984,-0.015]\n" +
            "  p9 = p[0.027,-0.482,-0.05,2.508,-1.984,-0.015]\n" +
            "  movej(p1, a=1.2, v=0.25)\n" +
            "  movej(p2, a=1.2, v=0.25)\n" +
            "  movel(p6, a=1.2, v=0.25)\n" +
            "  movel(p2, a=1.2, v=0.25)\n" +
            "  movej(p5, a=1.2, v=0.25)\n" +
            "  movel(p9, a=1.2, v=0.25)\n" +
            "  movel(p5, a=1.2, v=0.25)\n" +
            "  movej(p1, a=1.2, v=0.25)\n" +
            "end\n" +
            "prog()\n"
        );
    }

    public static void RunComponentB()
    {
        SendProgram(
            "def prog():\n" +
            "  p1 = p[0.130,-0.345,0.548,2.01,-0.001,-0.007]\n" +
            "  p3 = p[0.425,-0.225,0.044,3.146,-0.478,-0.001]\n" +
            "  p7 = p[0.425,-0.225,-0.125,3.146,-0.478,-0.001]\n" +
            "  p5 = p[0.027,-0.482,0.044,2.508,-1.984,-0.015]\n" +
            "  p9 = p[0.027,-0.482,-0.05,2.508,-1.984,-0.015]\n" +
            "  movej(p1, a=1.2, v=0.25)\n" +
            "  movej(p3, a=1.2, v=0.25)\n" +
            "  movel(p7, a=1.2, v=0.25)\n" +
            "  movel(p3, a=1.2, v=0.25)\n" +
            "  movej(p5, a=1.2, v=0.25)\n" +
            "  movel(p9, a=1.2, v=0.25)\n" +
            "  movel(p5, a=1.2, v=0.25)\n" +
            "  movej(p1, a=1.2, v=0.25)\n" +
            "end\n" +
            "prog()\n"
        );
    }

    public static void RunComponentC()
    {
        SendProgram(
            "def prog():\n" +
            "  p1 = p[0.130,-0.345,0.548,2.01,-0.001,-0.007]\n" +
            "  p4 = p[0.292,-0.385,0.044,2.972,-1.166,-0.041]\n" +
            "  p8 = p[0.292,-0.385,-0.125,2.972,-1.166,-0.041]\n" +
            "  p5 = p[0.027,-0.482,0.044,2.508,-1.984,-0.015]\n" +
            "  p9 = p[0.027,-0.482,-0.05,2.508,-1.984,-0.015]\n" +
            "  movej(p1, a=1.2, v=0.25)\n" +
            "  movej(p4, a=1.2, v=0.25)\n" +
            "  movel(p8, a=1.2, v=0.25)\n" +
            "  movel(p4, a=1.2, v=0.25)\n" +
            "  movej(p5, a=1.2, v=0.25)\n" +
            "  movel(p9, a=1.2, v=0.25)\n" +
            "  movel(p5, a=1.2, v=0.25)\n" +
            "  movej(p1, a=1.2, v=0.25)\n" +
            "end\n" +
            "prog()\n"
        );
    }
}
