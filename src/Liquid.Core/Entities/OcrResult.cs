using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Drawing;

namespace Liquid.Core.Entities
{
    /// <summary>
    /// Optical Character Recognition result set.
    /// </summary>
    [ExcludeFromCodeCoverage]
    public class OcrResult
    {
        /// <summary>
        /// Recognition result content.
        /// </summary>
        public string Content { get; set; }

        /// <summary>
        ///Analyzed pages.
        /// </summary>
        public List<PageInfo> Pages { get; set; }
    }
    /// <summary>
    /// Analyzed page content.
    /// </summary>
    [ExcludeFromCodeCoverage]
    public class PageInfo
    {
        /// <summary>
        /// recognition result page index.
        /// </summary>
        public int PageNumber { get; set; }

        /// <summary>
        /// The unit used by the words and lines data polygon properties.
        /// </summary>
        public string PolygonUnit { get; set; }

        /// <summary>
        /// Extracted words from the page.
        /// </summary>
        public List<WordData> Words { get; set; } = new List<WordData>();

        /// <summary>
        /// Extracted lines from the page.
        /// </summary>
        public List<LineData> Lines { get; set; } = new List<LineData>();
    }
    /// <summary>
    /// A word object consisting of a contiguous sequence of characters.
    /// </summary>
    [ExcludeFromCodeCoverage]
    public class WordData
    {
        /// <summary>
        /// Text content of the word.
        /// </summary>
        public string Content { get; set; }
        /// <summary>
        /// Confidence of correctly extracting the word.
        /// </summary>
        public float Confidence { get; set; }

        /// <summary>
        /// The polygon that outlines the content of this word. Coordinates are specified relative to the
        /// top-left of the page, and points are ordered clockwise from the left relative to the word
        /// orientation.
        /// </summary>
        public List<PointF> Polygon { get; set; } = new List<PointF>();
    }

    /// <summary>
    ///  A content line object consisting of an adjacent sequence of content elements
    /// </summary>
    [ExcludeFromCodeCoverage]
    public class LineData
    {
        /// <summary>
        /// Concatenated content of the contained elements in reading order.
        /// </summary>
        public string Content { get; set; }

        /// <summary>
        /// The polygon that outlines the content of this line. Coordinates are specified relative to the
        /// top-left of the page, and points are ordered clockwise from the left relative to the line
        /// orientation.
        /// </summary>
        public List<PointF> Polygon { get; set; } = new List<PointF>();

    }
}
