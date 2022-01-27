using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Shapes;

namespace Nulah.PhantomIndex.Core.Controls
{
    public class NulahFancyBorder : UserControl
    {
        static NulahFancyBorder()
        {
            DefaultStyleKeyProperty.OverrideMetadata(typeof(NulahFancyBorder), new FrameworkPropertyMetadata(typeof(NulahFancyBorder)));
        }

        protected override Size MeasureOverride(Size constraint)
        {
            var content = Content as UIElement;
            content.Measure(constraint);

            // Double the maximum size to make sure the safe margin for content is always content.DesiredSize
            return new Size(content.DesiredSize.Width + _maximumBound * 2, content.DesiredSize.Height + _maximumBound * 2);
        }

        protected override Size ArrangeOverride(Size arrangeBounds)
        {
            var minimumBoundWidth = arrangeBounds.Width + _minimumBound;
            var maximumBoundWidth = arrangeBounds.Width + _maximumBound;
            var minimumBoundHeight = arrangeBounds.Height + _minimumBound;
            var maximumBoundHeight = arrangeBounds.Height + _maximumBound;

            // Don't cache any random values for these as they need to all be random (or at least, independent from each other random)
            // Top left
            _borderPolygon.Points.Add(new Point(Random(_minimumBound, _maximumBound), Random(_minimumBound, _maximumBound)));
            // Top right
            _borderPolygon.Points.Add(new Point(Random(minimumBoundWidth, maximumBoundWidth), Random(_minimumBound, _maximumBound)));
            // Bottom right
            _borderPolygon.Points.Add(new Point(Random(minimumBoundWidth, maximumBoundWidth), Random(minimumBoundHeight, maximumBoundHeight)));
            // Bottom left
            _borderPolygon.Points.Add(new Point(Random(_minimumBound, _maximumBound), Random(minimumBoundHeight + _minimumBound, maximumBoundHeight)));

            return base.ArrangeOverride(arrangeBounds);
        }

        private Polygon _borderPolygon;
        private Random _random = new Random();
        /// <summary>
        /// Low value to use for random positions
        /// </summary>
        private double _minimumBound = -5;
        /// <summary>
        /// High value to use for random positions
        /// </summary>
        private double _maximumBound = 5;

        /// <summary>
        /// Returns a random number constrained to the <paramref name="min"/>/<paramref name="max"/> range
        /// </summary>
        /// <param name="min"></param>
        /// <param name="max"></param>
        /// <returns></returns>
        private double Random(double min, double max)
        {
            return _random.NextDouble() * (max - min) + min;
        }

        public override void OnApplyTemplate()
        {
            _borderPolygon = GetTemplateChild("FancyBorderPolygon") as Polygon;

            base.OnApplyTemplate();
        }
    }
}
