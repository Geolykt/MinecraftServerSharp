﻿using Mapping = MinecraftServerSharp.Network.Packets.PacketIDMappingAttribute;
using State = MinecraftServerSharp.Network.Packets.ProtocolState;

namespace MinecraftServerSharp.Network.Packets
{
    public enum ClientPacketID
    {
        Undefined,

        #region Handshaking

        [Mapping(0x00, State.Handshaking)] Handshake,
        [Mapping(0xfe, State.Handshaking)] LegacyServerListPing,

        #endregion

        #region Status

        Request,
        Ping,

        #endregion

        #region Login

        LoginStart,
        EncryptionResponse,
        LoginPluginResponse,

        #endregion

        #region Play

        [Mapping(0x03, State.Play)] ChatMessage
        
        #endregion
    }
}
