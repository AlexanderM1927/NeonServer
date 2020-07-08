using log4net;
using Neon.Core;
using Neon.HabboHotel.Users;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;

namespace Neon.HabboHotel.Cache.Process
{
    internal sealed class ProcessComponent
    {
        private static readonly ILog log = LogManager.GetLogger("Neon.HabboHotel.Cache.Process.ProcessComponent");

        /// <summary>
        /// ThreadPooled Timer.
        /// </summary>
        private Timer _timer = null;

        /// <summary>
        /// Prevents the timer from overlapping itself.
        /// </summary>
        private bool _timerRunning = false;

        /// <summary>
        /// Checks if the timer is lagging behind (server can't keep up).
        /// </summary>
#pragma warning disable CS0414 // O campo "ProcessComponent._timerLagging" é atribuído, mas seu valor nunca é usado
        private bool _timerLagging = false;
#pragma warning restore CS0414 // O campo "ProcessComponent._timerLagging" é atribuído, mas seu valor nunca é usado

        /// <summary>
        /// Enable/Disable the timer WITHOUT disabling the timer itself.
        /// </summary>
        private bool _disabled = false;

        /// <summary>
        /// Used for disposing the ProcessComponent safely.
        /// </summary>
        private readonly AutoResetEvent _resetEvent = new AutoResetEvent(true);

        /// <summary>
        /// How often the timer should execute.
        /// </summary>
        private static readonly int _runtimeInSec = 1200;

        /// <summary>
        /// Default.
        /// </summary>
        public ProcessComponent()
        {
        }

        /// <summary>
        /// Initializes the ProcessComponent.
        /// </summary>
        public void Init()
        {
            _timer = new Timer(new TimerCallback(Run), null, _runtimeInSec * 1000, _runtimeInSec * 1000);
        }

        /// <summary>
        /// Called for each time the timer ticks.
        /// </summary>
        /// <param name="State"></param>
        public void Run(object State)
        {
            try
            {
                if (_disabled)
                {
                    return;
                }

                if (_timerRunning)
                {
                    _timerLagging = true;
                    return;
                }

                _resetEvent.Reset();

                // BEGIN CODE
                List<UserCache> CacheList = NeonEnvironment.GetGame().GetCacheManager().GetUserCache().ToList();
                if (CacheList.Count > 0)
                {
                    foreach (UserCache Cache in CacheList)
                    {
                        try
                        {
                            if (Cache == null)
                            {
                                continue;
                            }

                            UserCache Temp = null;

                            if (Cache.isExpired())
                            {
                                NeonEnvironment.GetGame().GetCacheManager().TryRemoveUser(Cache.Id, out Temp);
                            }

                            Temp = null;
                        }
                        catch (Exception e)
                        {
                            Logging.LogCacheException(e.ToString());
                        }
                    }
                }

                CacheList = null;

                List<Habbo> CachedUsers = NeonEnvironment.GetUsersCached().ToList();
                if (CachedUsers.Count > 0)
                {
                    foreach (Habbo Data in CachedUsers)
                    {
                        try
                        {
                            if (Data == null)
                            {
                                continue;
                            }

                            Habbo Temp = null;

                            if (Data.CacheExpired())
                            {
                                NeonEnvironment.RemoveFromCache(Data.Id, out Temp);
                            }

                            if (Temp != null)
                            {
                                Temp.Dispose();
                            }

                            Temp = null;
                        }
                        catch (Exception e)
                        {
                            Logging.LogCacheException(e.ToString());
                        }
                    }
                }

                CachedUsers = null;
                // END CODE

                // Reset the values
                _timerRunning = false;
                _timerLagging = false;

                _resetEvent.Set();
            }
            catch (Exception e) { Logging.LogCacheException(e.ToString()); }
        }

        /// <summary>
        /// Stops the timer and disposes everything.
        /// </summary>
        public void Dispose()
        {
            // Wait until any processing is complete first.
            try
            {
                _resetEvent.WaitOne(TimeSpan.FromMinutes(5));
            }
            catch { } // give up

            // Set the timer to disabled
            _disabled = true;

            // Dispose the timer to disable it.
            try
            {
                if (_timer != null)
                {
                    _timer.Dispose();
                }
            }
            catch { }

            // Remove reference to the timer.
            _timer = null;
        }
    }
}