# Slipe Server

Slipe Server is a C# implementation of an MTA San Andreas Server.
The goal of this project is to make a more configurable and performant implementation of an MTA server and an environment to run C# code in, instead of being confined to Lua scripts.

This would allow for running native C# code on the server, ideally to be used in combination with [Slipe-Lua](https://github.com/mta-slipe/Slipe-core) for the client sided code.

## Net dll
MTA San Andreas' networking library (which is based on RakNet by Facebook) is sadly closed-source. This means that this project has to make use of MTA's net.dll to be compatible with the official MTA client.

This is done using `DllImport`s from C# code, calling a C++ wrapper around MTA's Net Library. 

## Packets
All packet definitions are to be recreated in the C# code. These packets use the `PacketBuilder` and `PacketReader` classes to represent the net library's bitstream. These classes also contain some methods for writing some MTA-specific structures used in the network packets.
