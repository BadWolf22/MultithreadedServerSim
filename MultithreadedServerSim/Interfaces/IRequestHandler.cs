﻿using MultithreadedServerSim.Contracts;

namespace MultithreadedServerSim.Interfaces;

internal interface IRequestHandler
{
    Response HandleRequest(dynamic details);
}
