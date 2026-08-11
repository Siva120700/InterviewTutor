---
id: net-tcp-udp
title: TCP vs UDP
track: cs-networking
module: "01 Application"
order: 2
languages: [java, csharp]
summary: Reliability, ordering, congestion control, and when to build reliability on UDP.
---

## Why this matters

System design follow-ups (“why not UDP for chat?”) test whether you understand transport trade-offs.

## Definitions

- **TCP:** Connection-oriented transport delivering a reliable, ordered **byte stream** with ACKs, retransmission, and congestion control.
- **UDP:** Connectionless **datagram** transport with no built-in reliability, ordering, or congestion control — the app owns those policies.
- **Three-way handshake:** SYN → SYN-ACK → ACK exchange that establishes a TCP connection before data transfer.
- **Congestion control:** Sender slows when the network is overloaded so shared paths stay usable (distinct from flow control).
- **Flow control:** Limit send rate based on the **receiver’s** available buffer space.
- **Head-of-line blocking:** A lost/delayed packet stalls delivery of later data waiting behind it on the same stream/connection.
- **Datagram:** Self-contained UDP packet whose boundaries the network preserves (unlike TCP’s continuous byte stream).
- **QUIC:** Modern transport usually over UDP combining encryption with multiplexed streams (HTTP/3) to reduce TCP-style HOL issues.

## Comparison

| | TCP | UDP |
|---|-----|-----|
| Connection | Stream, handshake | Datagrams |
| Reliability | Ack/retransmit | None |
| Ordering | Ordered byte stream | Not ordered |
| Congestion control | Yes | App’s problem |
| Use cases | HTTP, DB, SSH | DNS, games, WebRTC media, QUIC under H3 |

## TCP mental model

- 3-way handshake, flow control, congestion window  
- Head-of-line blocking: lost packet stalls later data  
- Great when correctness > latency spikes

## UDP mental model

- Send packet, hope  
- App may add seq numbers, NACKs, FEC  
- Prefer freshness (video frame) over perfect replay

## Worked example — sockets (illustrative)

```java
try (ServerSocket server = new ServerSocket(8080);
     Socket sock = server.accept();
     var in = new BufferedReader(new InputStreamReader(sock.getInputStream()))) {
  System.out.println(in.readLine());
}
```

```csharp
using var listener = new TcpListener(IPAddress.Any, 8080);
listener.Start();
using var client = await listener.AcceptTcpClientAsync();
using var stream = client.GetStream();
// read bytes…
```

UDP:

```java
DatagramSocket socket = new DatagramSocket(9050);
byte[] buf = new byte[1024];
DatagramPacket packet = new DatagramPacket(buf, buf.length);
socket.receive(packet);
```

```csharp
using var udp = new UdpClient(9050);
var result = await udp.ReceiveAsync();
```

## Interview Q&A

- **Q:** Is UDP always faster?
  **A:** No — without congestion control you can melt networks; “faster” means lower latency when loss/retransmit hurts more than drops.
- **Q:** Why does HTTP/3 use QUIC/UDP?
  **A:** Avoid TCP HOL; combine crypto+transport; better connection migration.
- **Q:** DNS over UDP?
  **A:** Classic small queries; fall back to TCP for large responses.

## Pitfalls

- Assuming TCP messages preserve app “message boundaries” (it’s a byte stream — you need framing)  
- Forgetting NAT timeouts for idle UDP  
- Reimplementing TCP poorly on UDP

## 60-second answer

“TCP gives reliable ordered streams with congestion control — default for APIs. UDP is for latency-sensitive or multiplexed transports where the app (or QUIC) owns reliability. I’d never say UDP is universally faster.”

## Further study

- [TCP (MDN Glossary)](https://developer.mozilla.org/en-US/docs/Glossary/TCP) — concise, interview-friendly TCP definition
- [UDP (MDN Glossary)](https://developer.mozilla.org/en-US/docs/Glossary/UDP) — datagram model and what apps must own
- [RFC 9293 — TCP](https://www.rfc-editor.org/rfc/rfc9293.html) — authoritative TCP specification (handshake, reliability)
- [QUIC (MDN Glossary)](https://developer.mozilla.org/en-US/docs/Glossary/QUIC) — UDP-based transport behind HTTP/3

## Practice prompts

1. Design a simple length-prefixed TCP protocol  
2. When would you pick gRPC vs WebSocket vs UDP  
3. Explain TCP retransmission impact on tail latency
