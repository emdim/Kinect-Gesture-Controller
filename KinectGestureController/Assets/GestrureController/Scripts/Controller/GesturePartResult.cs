﻿using System;

namespace GesturesInput
{
    /// <summary>
    /// Represents the gesture part recognition result.
    /// </summary>
    public enum GesturePartResult
    {
        /// <summary>
        /// Gesture part failed.
        /// </summary>
        Failed,

        /// <summary>
        /// Gesture part succeeded.
        /// </summary>
        Succeeded,

        /// <summary>
        /// Gesture part undetermined and pausing detection.
        /// </summary>
        Pausing
    }
}
