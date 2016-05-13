/*
$Id: 25da63d876d4eeb6710437b88024567cd1b64944 $

This file is part of the iText (R) project.
Copyright (c) 1998-2016 iText Group NV
Authors: Bruno Lowagie, Paulo Soares, et al.

This program is free software; you can redistribute it and/or modify
it under the terms of the GNU Affero General Public License version 3
as published by the Free Software Foundation with the addition of the
following permission added to Section 15 as permitted in Section 7(a):
FOR ANY PART OF THE COVERED WORK IN WHICH THE COPYRIGHT IS OWNED BY
ITEXT GROUP. ITEXT GROUP DISCLAIMS THE WARRANTY OF NON INFRINGEMENT
OF THIRD PARTY RIGHTS

This program is distributed in the hope that it will be useful, but
WITHOUT ANY WARRANTY; without even the implied warranty of MERCHANTABILITY
or FITNESS FOR A PARTICULAR PURPOSE.
See the GNU Affero General Public License for more details.
You should have received a copy of the GNU Affero General Public License
along with this program; if not, see http://www.gnu.org/licenses or write to
the Free Software Foundation, Inc., 51 Franklin Street, Fifth Floor,
Boston, MA, 02110-1301 USA, or download the license from the following URL:
http://itextpdf.com/terms-of-use/

The interactive user interfaces in modified source and object code versions
of this program must display Appropriate Legal Notices, as required under
Section 5 of the GNU Affero General Public License.

In accordance with Section 7(b) of the GNU Affero General Public License,
a covered work must retain the producer line in every PDF that is created
or manipulated using iText.

You can be released from the requirements of the license by purchasing
a commercial license. Buying such a license is mandatory as soon as you
develop commercial activities involving the iText software without
disclosing the source code of your own applications.
These activities include: offering paid services to customers as an ASP,
serving PDFs on the fly in a web application, shipping iText with a closed
source product.

For more information, please contact iText Software Corp. at this
address: sales@itextpdf.com
*/
using System;
using com.itextpdf.kernel.color;
using com.itextpdf.kernel.pdf.canvas;

namespace com.itextpdf.layout.border
{
	public abstract class Border
	{
		public static readonly com.itextpdf.layout.border.Border NO_BORDER = null;

		public const int SOLID = 0;

		public const int DASHED = 1;

		public const int DOTTED = 2;

		public const int DOUBLE = 3;

		public const int ROUND_DOTS = 4;

		public const int _3D_GROOVE = 5;

		public const int _3D_INSET = 6;

		public const int _3D_OUTSET = 7;

		public const int _3D_RIDGE = 8;

		protected internal Color color;

		protected internal float width;

		protected internal int type;

		private int hash;

		public Border(float width)
			: this(Color.BLACK, width)
		{
		}

		public Border(Color color, float width)
		{
			this.color = color;
			this.width = width;
		}

		/// <summary>
		/// <p>
		/// All borders are supposed to be drawn in such way, that inner content of the element is on the right from the
		/// drawing direction.
		/// </summary>
		/// <remarks>
		/// <p>
		/// All borders are supposed to be drawn in such way, that inner content of the element is on the right from the
		/// drawing direction. Borders are drawn in this order: top, right, bottom, left.
		/// </p>
		/// <p>
		/// Given points specify the line which lies on the border of the content area,
		/// therefore the border itself should be drawn to the left from the drawing direction.
		/// </p>
		/// <p>
		/// <code>borderWidthBefore</code> and <code>borderWidthAfter</code> parameters are used to
		/// define the widths of the borders that are before and after the current border, e.g. for
		/// the bottom border, <code>borderWidthBefore</code> specifies width of the right border and
		/// <code>borderWidthAfter</code> - width of the left border. Those width are used to handle areas
		/// of border joins.
		/// </p>
		/// </remarks>
		/// <param name="canvas">PdfCanvas to be written to</param>
		/// <param name="x1">x coordinate of the beginning point of the element side, that should be bordered
		/// 	</param>
		/// <param name="y1">y coordinate of the beginning point of the element side, that should be bordered
		/// 	</param>
		/// <param name="x2">x coordinate of the ending point of the element side, that should be bordered
		/// 	</param>
		/// <param name="y2">y coordinate of the ending point of the element side, that should be bordered
		/// 	</param>
		/// <param name="borderWidthBefore">defines width of the border that is before the current one
		/// 	</param>
		/// <param name="borderWidthAfter">defines width of the border that is after the current one
		/// 	</param>
		public abstract void Draw(PdfCanvas canvas, float x1, float y1, float x2, float y2
			, float borderWidthBefore, float borderWidthAfter);

		public abstract void DrawCellBorder(PdfCanvas canvas, float x1, float y1, float x2
			, float y2);

		public abstract int GetType();

		public virtual Color GetColor()
		{
			return color;
		}

		public virtual float GetWidth()
		{
			return width;
		}

		public override bool Equals(Object anObject)
		{
			if (this == anObject)
			{
				return true;
			}
			if (anObject is com.itextpdf.layout.border.Border)
			{
				com.itextpdf.layout.border.Border anotherBorder = (com.itextpdf.layout.border.Border
					)anObject;
				if (anotherBorder.GetType() != GetType() || anotherBorder.GetColor() != GetColor(
					) || anotherBorder.GetWidth() != GetWidth())
				{
					return false;
				}
			}
			else
			{
				return false;
			}
			return true;
		}

		public override int GetHashCode()
		{
			int h = hash;
			if (h == 0)
			{
				h = (int)GetWidth() * 31 + GetColor().GetHashCode();
				hash = h;
			}
			return h;
		}

		protected internal virtual Border.Side GetBorderSide(float x1, float y1, float x2
			, float y2)
		{
			bool isLeft = false;
			bool isRight = false;
			if (Math.Abs(y2 - y1) > 0.0005f)
			{
				isLeft = y2 - y1 > 0;
				isRight = y2 - y1 < 0;
			}
			bool isTop = false;
			bool isBottom = false;
			if (Math.Abs(x2 - x1) > 0.0005f)
			{
				isTop = x2 - x1 > 0;
				isBottom = x2 - x1 < 0;
			}
			if (isTop)
			{
				return Border.Side.TOP;
			}
			else
			{
				if (isRight)
				{
					return Border.Side.RIGHT;
				}
				else
				{
					if (isBottom)
					{
						return Border.Side.BOTTOM;
					}
					else
					{
						if (isLeft)
						{
							return Border.Side.LEFT;
						}
					}
				}
			}
			return Border.Side.NONE;
		}

		protected internal enum Side
		{
			NONE,
			TOP,
			RIGHT,
			BOTTOM,
			LEFT
		}
	}
}