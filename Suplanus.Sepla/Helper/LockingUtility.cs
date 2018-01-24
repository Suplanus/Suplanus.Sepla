using System;
using Eplan.EplApi.DataModel;

namespace Suplanus.Sepla.Helper
{
  public class LockingUtility
  {
    public class SeplaLockingVector : IDisposable
    {
      private readonly int _manualLockStateId;
      private readonly LockingVector _lockingVector;

      public SeplaLockingVector()
      {
        _lockingVector = new LockingVector();
        _manualLockStateId = _lockingVector.PauseManualLock();
      }


      public void Dispose()
      {
        _lockingVector.ResumeManualLock(_manualLockStateId);
        _lockingVector.Dispose();
      }
    }
  }
}