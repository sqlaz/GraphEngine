// Graph Engine
// Copyright (c) Microsoft. All rights reserved.
// Licensed under the MIT license. See LICENSE.md file in the project root for full license information.
//
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Runtime.ConstrainedExecution;
using System.Runtime;

namespace Trinity.Core.Lib
{
#pragma warning disable 0420
    internal static class SpinLockSlim
    {
        static SpinLockSlim()
        {
            InternalCalls.__init();
        }

        internal static void GetLock(ref int spinlock)
        {
            if (Interlocked.CompareExchange(ref spinlock, 1, 0) != 0)
            {
                while (true)
                {
                    if (spinlock == 0 && Interlocked.CompareExchange(ref spinlock, 1, 0) == 0)
                        return;
                    Thread.Sleep(0);
                }
            }
        }

        [ReliabilityContract(Consistency.WillNotCorruptState, Cer.Success), TargetedPatchingOptOut("Performance critical to inline across NGen image boundaries")]
        internal static void UnLock(ref int spinlock)
        {
            Thread.MemoryBarrier();
            spinlock = 0;
        }
    }
}
