using System.Net.Sockets;
using System.Text;

namespace Login;

public static class RobotConnectionTest
{
    private const string RobotIp = "172.20.254.201";   // <-- brug den IP som VIRKEDE før
    private const int UrScriptPort = 30002;
    private const int DashboardPort = 29999;

    // Sender HELE scriptet i én TCP-forbindelse (vigtigt!)
    private static void SendString(int port, string message)
    {
        using var client = new TcpClient(RobotIp, UrScriptPort);
        using var stream = client.GetStream();

        var bytes = Encoding.ASCII.GetBytes(message);
        stream.Write(bytes, 0, bytes.Length);
        stream.Flush();
    }

     // URScript: send HELE scriptet i én forbindelse
    private static void SendProgram(string urScriptProgram)
    {
        SendString(UrScriptPort, urScriptProgram.EndsWith("\n") ? urScriptProgram : urScriptProgram + "\n");
    }

    // --- Dashboard ---
    public static void BrakeRelease() => SendString(DashboardPort, "brake release\n");
    public static void StopRobot() => SendString(DashboardPort, "stop\n");

    // ✅ Sanity test
    public static void MoveToP2_Single()
    {
        SendProgram(
            "def prog():\n" +
            "  movej(p[0.482,-0.118,0.044,3.182,-0.003,-0.009], a=1.2, v=0.25)\n" +
            "end\n" +
            "prog()\n"
        );
    }

    // --------- Kør komponenter ----------
    public static void RunComponentA() => SendProgram(BuildProgram("A"));
    public static void RunComponentB() => SendProgram(BuildProgram("B"));
    public static void RunComponentC() => SendProgram(BuildProgram("C"));
    public static void RunAll()        => SendProgram(BuildProgram("ALL"));

    // --------- 1 samlet URScript (vælger A/B/C/ALL) ----------
    private static string BuildProgram(string mode)
    {
        mode = (mode ?? "ALL").Trim().ToUpperInvariant();
        if (mode is not ("A" or "B" or "C" or "ALL")) mode = "ALL";

        // NB: Jeg har fjernet get_inverse_kin(...) for at matche dine fungerende tests
        // (du bruger allerede movej(p[...]) og movel(p[...])).
        return
            "def prog():\n" +
            "  p1 = p[0.130,-0.345,0.548,2.01,-0.001,-0.007]\n" +
            "  p2 = p[0.482,-0.118,0.044,3.182,-0.003,-0.009]\n" +
            "  p6 = p[0.482,-0.118,-0.125,3.182,-0.003,-0.009]\n" +
            "  p3 = p[0.425,-0.225,0.044,3.146,-0.478,-0.001]\n" +
            "  p7 = p[0.425,-0.225,-0.125,3.146,-0.478,-0.001]\n" +
            "  p4 = p[0.292,-0.385,0.044,2.972,-1.166,-0.041]\n" +
            "  p8 = p[0.292,-0.385,-0.125,2.972,-1.166,-0.041]\n" +
            "  p5 = p[0.027,-0.482,0.044,2.508,-1.984,-0.015]\n" +
            "  p9 = p[0.027,-0.482,-0.05,2.508,-1.984,-0.015]\n" +
            "\n" +
            "  a = 1.2\n" +
            "  v = 0.25\n" +
            "\n" +
            "  # Robotiq via xmlrpc (behold kun hvis det virker på din robot)\n" +
            "  global RPC = rpc_factory(\"xmlrpc\", \"http://172.20.254.201:41414\")\n" +
            "  global TOOL_INDEX = 0\n" +
            "  def rg_is_busy():\n" +
            "    return RPC.rg_get_busy(TOOL_INDEX)\n" +
            "  end\n" +
            "  def rg_grip(width, force=20):\n" +
            "    RPC.rg_grip(TOOL_INDEX, width + 0.0, force + 0.0)\n" +
            "    sleep(0.01)\n" +
            "    while (rg_is_busy()):\n" +
            "      sleep(0.01)\n" +
            "    end\n" +
            "  end\n" +
            "\n" +
            "  def do_A():\n" +
            "    movej(p1, a=a, v=v)\n" +
            "    movej(p2, a=a, v=v)\n" +
            "    movel(p6, a=a, v=v)\n" +
            "    rg_grip(50)\n" +
            "    rg_grip(32)\n" +
            "    movel(p2, a=a, v=v)\n" +
            "    movej(p5, a=a, v=v)\n" +
            "    movel(p9, a=a, v=v)\n" +
            "    rg_grip(50)\n" +
            "    movel(p5, a=a, v=v)\n" +
            "    movej(p1, a=a, v=v)\n" +
            "  end\n" +
            "\n" +
            "  def do_B():\n" +
            "    movej(p1, a=a, v=v)\n" +
            "    movej(p3, a=a, v=v)\n" +
            "    movel(p7, a=a, v=v)\n" +
            "    rg_grip(50)\n" +
            "    rg_grip(11)\n" +
            "    movel(p3, a=a, v=v)\n" +
            "    movej(p5, a=a, v=v)\n" +
            "    movel(p9, a=a, v=v)\n" +
            "    rg_grip(50)\n" +
            "    movel(p5, a=a, v=v)\n" +
            "    movej(p1, a=a, v=v)\n" +
            "  end\n" +
            "\n" +
            "  def do_C():\n" +
            "    movej(p1, a=a, v=v)\n" +
            "    movej(p4, a=a, v=v)\n" +
            "    movel(p8, a=a, v=v)\n" +
            "    rg_grip(50)\n" +
            "    rg_grip(32)\n" +
            "    movel(p4, a=a, v=v)\n" +
            "    movej(p5, a=a, v=v)\n" +
            "    movel(p9, a=a, v=v)\n" +
            "    rg_grip(50)\n" +
            "    movel(p5, a=a, v=v)\n" +
            "    movej(p1, a=a, v=v)\n" +
            "  end\n" +
            "\n" +
            $"  if (\"{mode}\" == \"A\"):\n" +
            "    do_A()\n" +
            $"  elif (\"{mode}\" == \"B\"):\n" +
            "    do_B()\n" +
            $"  elif (\"{mode}\" == \"C\"):\n" +
            "    do_C()\n" +
            "  else:\n" +
            "    do_A()\n" +
            "    do_B()\n" +
            "    do_C()\n" +
            "  end\n" +
            "end\n" +
            "prog()\n";
    }
}
