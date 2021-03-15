using System;
using System.Collections.Generic;
using System.Text;

namespace SpaceExodus_Server
{
    class Constants
    {
        public const int TICS_PER_SEC = 30;
        public const int MS_PER_TICK = 1000 / TICS_PER_SEC;
        public const float DEG_2_RAD = (MathF.PI * 2) / 360.0f;
    }
}
