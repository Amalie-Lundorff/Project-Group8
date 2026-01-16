using System.Net.Sockets;
using System.Text;

const string program = @"
def f():
  Startposition = p[.130, -.345, .548, 2.01, -.001, -.007]

# === Komponent A ===
  componentA_pick_high = p[.482, -.118, .044, 3.182, -.003, -.009]
  componentA_pick_low = p[ 0.482, -0.118, -.125, 3.182, -0.003, -0.009]

# === Komponent B ===
  componentB_pick_high = p[.425, -.225, .044, 3.146, -.478, -.001] 
  componentB_pick_low = p[ 0.425, -0.225, -.125, 3.146, -0.478, -0.001]

# === Komponent C ===
  componentC_pick_high = p[.292, -.385, .044, 2.972, -1.166, -.041]
  componentC_pick_low = p[ 0.292, -0.385, -.125, 2.972, -1.166, -0.041]

# === Pakkestation ===
  pack_place_high = p[.027, -.482, .044, 2.508, -1.984, -.015]
  pack_place_low = p[ 0.027, -0.482, -.05, 2.508, -1.984, -.015]


 global RPC = rpc_factory(""""xmlrpc"""", """"http://172.20.254.203:41414"""")
  global TOOL_INDEX = 0

  def rg_is_busy():
    return RPC.rg_get_busy(TOOL_INDEX)
  end

  def rg_grip(width, force=20):
    RPC.rg_grip(TOOL_INDEX, width + 0.0, force + 0.0)
    sleep(0.01)
    while (rg_is_busy()):
      sleep(0.01)
    end
  end

  times = 0
  while (times < 1):
    movej(get_inverse_kin(Startposition))

def run_component_A():
    movej(get_inverse_kin(componentA_pick_high))   # komponent (høj)
    movel(get_inverse_kin(componentA_pick_low))   # ned (lav)

    rg_grip(50)                  # gripper (åben)
    rg_grip(32)                  # gripper (luk)

    movel(get_inverse_kin(componentA_pick_high))   # op (høj)

    movej(get_inverse_kin(pack_place_high))   # pakke (høj)
    movel(get_inverse_kin(pack_place_low))   # ned (lav)

    rg_grip(50)                  # gripper (åben / slip)

    movel(get_inverse_kin(pack_place_high))   # op (høj)
    movej(get_inverse_kin(Startposition))   # start
end

def run_component_B():
    movej(get_inverse_kin(componentB_pick_high))   # komponent (høj)
    movel(get_inverse_kin(componentB_pick_low))   # ned (lav)

    rg_grip(50)                  # gripper (åben)
    rg_grip(11)                  # gripper (luk)

    movel(get_inverse_kin(componentB_pick_high))   # op (høj)

    movej(get_inverse_kin(pack_place_high))   # pakke (høj)
    movel(get_inverse_kin(pack_place_low))   # ned (lav)

    rg_grip(50)                  # gripper (åben / slip)

    movel(get_inverse_kin(pack_place_high))   # op (høj)
    movej(get_inverse_kin(Startposition))   # start
end

def run_component_C():
    movej(get_inverse_kin(componentC_pick_high))   # komponent (høj)
    movel(get_inverse_kin(componentC_pick_low))   # ned (lav)

    rg_grip(50)                  # gripper (åben)
    rg_grip(32)                  # gripper (luk)

    movel(get_inverse_kin(componentC_pick_high))   # op (høj)

    movej(get_inverse_kin(pack_place_high))   # pakke (høj)
    movel(get_inverse_kin(pack_place_low))   # ned (lav)

    rg_grip(50)                  # gripper (åben / slip)

    movel(get_inverse_kin(pack_place_high))   # op (høj)
    movej(get_inverse_kin(Startposition))   # start
    times = times + 1
  end
end
";

const int urscriptPort = 30002, dashboardPort = 29999;
const string IpAddress = "172.20.254.203";

void SendString(string host, int port, string message)
{
    using var client = new TcpClient(host, port);
    using var stream = client.GetStream();
    stream.Write(Encoding.ASCII.GetBytes(message));
}

SendString(IpAddress, dashboardPort, "brake release\n");
SendString(IpAddress, urscriptPort, program);

// To stop:
