using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ScorpionEngine.Content
{
    /// <summary>
    /// Represents reasons for an invalid animation meta data formats.
    /// </summary>
    public enum InValidReason
    {
        TooManyLeftBrackets = 0,
        TooManyRightBrackets = 1,
        TooManyDashes = 2,
        LeftBracketNotFirstChar = 3,
        RightBracketNotLastChar = 4,
        BracketOrderIncorrect = 5,
        DashLocationIncorrect = 6,
        SubTextureIDNotAlphaNumeric = 7,
        FrameIndexNotWholeNumber = 8,
        FrameIndexNotSequential = 9,
        ValidFormat = 10
    }
}
