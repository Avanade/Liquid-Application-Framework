using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Liquid.Core.GenAi.Enums
{
    /// <summary>
    /// Specifies the role of a participant in a message exchange, such as a user, assistant, or system.
    /// </summary>
    public enum LiquidMessageRole
    {
        /// <summary>
        /// The user role, typically representing the end user or client.
        /// </summary>
        User,
        /// <summary>
        /// The assistant role, typically representing the AI or system responding to the user.
        /// </summary>
        Assistant,
        /// <summary>
        /// The system role, typically used for system-level messages or instructions.
        /// </summary>
        System
    }
}
