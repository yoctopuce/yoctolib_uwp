/*********************************************************************
 *
 * $Id: YDisplayLayer.cs 74506 2026-06-01 15:57:01Z seb $
 *
 * Implements FindDisplayLayer(), the high-level API for DisplayLayer functions
 *
 * - - - - - - - - - License information: - - - - - - - - -
 *
 *  Copyright (C) 2011 and beyond by Yoctopuce Sarl, Switzerland.
 *
 *  Yoctopuce Sarl (hereafter Licensor) grants to you a perpetual
 *  non-exclusive license to use, modify, copy and integrate this
 *  file into your software for the sole purpose of interfacing
 *  with Yoctopuce products.
 *
 *  You may reproduce and distribute copies of this file in
 *  source or object form, as long as the sole purpose of this
 *  code is to interface with Yoctopuce products. You must retain
 *  this notice in the distributed source file.
 *
 *  You should refer to Yoctopuce General Terms and Conditions
 *  for additional information regarding your rights and
 *  obligations.
 *
 *  THE SOFTWARE AND DOCUMENTATION ARE PROVIDED 'AS IS' WITHOUT
 *  WARRANTY OF ANY KIND, EITHER EXPRESS OR IMPLIED, INCLUDING
 *  WITHOUT LIMITATION, ANY WARRANTY OF MERCHANTABILITY, FITNESS
 *  FOR A PARTICULAR PURPOSE, TITLE AND NON-INFRINGEMENT. IN NO
 *  EVENT SHALL LICENSOR BE LIABLE FOR ANY INCIDENTAL, SPECIAL,
 *  INDIRECT OR CONSEQUENTIAL DAMAGES, LOST PROFITS OR LOST DATA,
 *  COST OF PROCUREMENT OF SUBSTITUTE GOODS, TECHNOLOGY OR
 *  SERVICES, ANY CLAIMS BY THIRD PARTIES (INCLUDING BUT NOT
 *  LIMITED TO ANY DEFENSE THEREOF), ANY CLAIMS FOR INDEMNITY OR
 *  CONTRIBUTION, OR OTHER SIMILAR COSTS, WHETHER ASSERTED ON THE
 *  BASIS OF CONTRACT, TORT (INCLUDING NEGLIGENCE), BREACH OF
 *  WARRANTY, OR OTHERWISE.
 *
 *********************************************************************/

using System;
using System.Text;
using System.Threading.Tasks;

namespace com.yoctopuce.YoctoAPI
{


    //--- (generated code: YDisplayLayer return codes)
//--- (end of generated code: YDisplayLayer return codes)
    //--- (generated code: YDisplayLayer class start)
/**
 * <summary>
 *   YDisplayLayer Class: Interface for drawing into display layers, obtained by calling <c>display.get_displayLayer</c>.
 * <para>
 * </para>
 * <para>
 *   Each <c>DisplayLayer</c> represents an image layer containing objects
 *   to display (bitmaps, text, etc.). The content is displayed only when
 *   the layer is active on the screen (and not masked by other
 *   overlapping layers).
 * </para>
 * </summary>
 */
public class YDisplayLayer
{
//--- (end of generated code: YDisplayLayer class start)
        //--- (generated code: YDisplayLayer definitions)
    public const int NO_INK = -1;
    public const int BG_INK = -2;
    public const int FG_INK = -3;
    public enum ALIGN {
        TOP_LEFT = 0,
        CENTER_LEFT = 1,
        BASELINE_LEFT = 2,
        BOTTOM_LEFT = 3,
        TOP_CENTER = 4,
        CENTER = 5,
        BASELINE_CENTER = 6,
        BOTTOM_CENTER = 7,
        TOP_DECIMAL = 8,
        CENTER_DECIMAL = 9,
        BASELINE_DECIMAL = 10,
        BOTTOM_DECIMAL = 11,
        TOP_RIGHT = 12,
        CENTER_RIGHT = 13,
        BASELINE_RIGHT = 14,
        BOTTOM_RIGHT = 15}

    protected string _cmdbuff = "";
    protected bool _hidden = false;
    protected int _polyPrevX = 0;
    protected int _polyPrevY = 0;

    //--- (end of generated code: YDisplayLayer definitions)

        private YDisplay _display;
        private int _id;

        internal YDisplayLayer(YDisplay parent, int id)
        {
            //--- (generated code: YDisplayLayer attributes initialization)
        //--- (end of generated code: YDisplayLayer attributes initialization)
            _display = parent;
            _id = id;
        }



        //--- (generated code: YDisplayLayer implementation)
#pragma warning disable 1998

    public virtual bool must_be_flushed()
    {
        return (_cmdbuff).Length > 0;
    }

    public virtual int resetHiddenFlag()
    {
        _hidden = false;
        return YAPI.SUCCESS;
    }

    public virtual async Task<int> flush_now()
    {
        int res;
        res = YAPI.SUCCESS;
        if ((_cmdbuff).Length > 0) {
            res = await _display.sendCommand(_cmdbuff);
            _cmdbuff = "";
        }
        return res;
    }

    public virtual async Task<int> command_push(string cmd)
    {
        int res;
        res = YAPI.SUCCESS;
        if ((_cmdbuff).Length + (cmd).Length >= 100) {
            // force flush before, to prevent overflow
            await this.flush_now();
        }
        if ((_cmdbuff).Length == 0) {
            // always prepend layer ID first
            _cmdbuff = (_id).ToString();
        }
        _cmdbuff = _cmdbuff + cmd;
        return res;
    }

    public virtual async Task<int> command_flush(string cmd)
    {
        int res;

        res = await this.command_push(cmd);
        if (_hidden) {
            return res;
        }
        if (_display.isFrozen()) {
            return res;
        }
        return await this.flush_now();
    }

    /**
     * <summary>
     *   Reverts the layer to its initial state (fully transparent, default settings).
     * <para>
     *   Reinitializes the drawing pointer to the upper left position,
     *   and selects the most visible pen color. If you only want to erase the layer
     *   content, use the method <c>clear()</c> instead.
     * </para>
     * </summary>
     * <returns>
     *   <c>YAPI.SUCCESS</c> if the call succeeds.
     * </returns>
     * <para>
     *   On failure, throws an exception or returns a negative error code.
     * </para>
     */
    public virtual async Task<int> reset()
    {
        _hidden = false;
        return await this.command_flush("X");
    }

    /**
     * <summary>
     *   Erases the whole content of the layer (makes it fully transparent).
     * <para>
     *   This method does not change any other attribute of the layer.
     *   To reinitialize the layer attributes to defaults settings, use the method
     *   <c>reset()</c> instead.
     * </para>
     * </summary>
     * <returns>
     *   <c>YAPI.SUCCESS</c> if the call succeeds.
     * </returns>
     * <para>
     *   On failure, throws an exception or returns a negative error code.
     * </para>
     */
    public virtual async Task<int> clear()
    {
        return await this.command_flush("x");
    }

    /**
     * <summary>
     *   Selects the color to be used for all subsequent drawing functions,
     *   for filling as well as for line and text drawing.
     * <para>
     *   To select a different fill and outline color, use
     *   <c>selectFillColor</c> and <c>selectLineColor</c>.
     *   The pen color is provided as an RGB value.
     *   For grayscale or monochrome displays, the value is
     *   automatically converted to the proper range.
     * </para>
     * </summary>
     * <param name="color">
     *   the desired pen color, as a 24-bit RGB value
     * </param>
     * <returns>
     *   <c>YAPI.SUCCESS</c> if the call succeeds.
     * </returns>
     * <para>
     *   On failure, throws an exception or returns a negative error code.
     * </para>
     */
    public virtual async Task<int> selectColorPen(int color)
    {
        return await this.command_push("c"+String.Format("{0:x06}",color));
    }

    /**
     * <summary>
     *   Selects the pen gray level for all subsequent drawing functions,
     *   for filling as well as for line and text drawing.
     * <para>
     *   To select a different fill and outline color, use
     *   <c>selectFillColor</c> and <c>selectLineColor</c>.
     *   The gray level is provided as a number between
     *   0 (black) and 255 (white, or whichever the lightest color is).
     *   For monochrome displays (without gray levels), any value
     *   lower than 128 is rendered as black, and any value equal
     *   or above to 128 is non-black.
     * </para>
     * </summary>
     * <param name="graylevel">
     *   the desired gray level, from 0 to 255
     * </param>
     * <returns>
     *   <c>YAPI.SUCCESS</c> if the call succeeds.
     * </returns>
     * <para>
     *   On failure, throws an exception or returns a negative error code.
     * </para>
     */
    public virtual async Task<int> selectGrayPen(int graylevel)
    {
        return await this.command_push("g"+Convert.ToString(graylevel));
    }

    /**
     * <summary>
     *   Selects an eraser instead of a pen for all subsequent drawing functions,
     *   except for bitmap copy functions.
     * <para>
     *   Any point drawn using the eraser
     *   becomes transparent (as when the layer is empty), showing the other
     *   layers beneath it.
     * </para>
     * </summary>
     * <returns>
     *   <c>YAPI.SUCCESS</c> if the call succeeds.
     * </returns>
     * <para>
     *   On failure, throws an exception or returns a negative error code.
     * </para>
     */
    public virtual async Task<int> selectEraser()
    {
        return await this.command_push("e");
    }

    /**
     * <summary>
     *   Selects the color to be used for filling rectangular bars,
     *   discs and polygons.
     * <para>
     *   The color is provided as an RGB value.
     *   For grayscale or monochrome displays, the value is
     *   automatically converted to the proper range.
     *   You can also use the constants <c>FG_INK</c> to use the
     *   default drawing colour, <c>BG_INK</c> to use the default
     *   background colour, and <c>NO_INK</c> to disable filling.
     * </para>
     * </summary>
     * <param name="color">
     *   the desired drawing color, as a 24-bit RGB value,
     *   or one of the constants <c>NO_INK</c>, <c>FG_INK</c>
     *   or <c>BG_INK</c>
     * </param>
     * <returns>
     *   <c>YAPI.SUCCESS</c> if the call succeeds.
     * </returns>
     * <para>
     *   On failure, throws an exception or returns a negative error code.
     * </para>
     */
    public virtual async Task<int> selectFillColor(int color)
    {
        int r;
        int g;
        int b;
        if (color==-1) {
            return await this.command_push("f_");
        }
        if (color==-2) {
            return await this.command_push("f-");
        }
        if (color==-3) {
            return await this.command_push("f.");
        }
        r = ((color >> 20) & 15);
        g = ((color >> 12) & 15);
        b = ((color >> 4) & 15);
        return await this.command_push("f"+String.Format("{0:x}",r)+""+String.Format("{0:x}",g)+""+String.Format("{0:x}",b));
    }

    /**
     * <summary>
     *   Selects the color to be used for drawing the outline of rectangular
     *   bars, discs and polygons, as well as for drawing lines and text.
     * <para>
     *   The color is provided as an RGB value.
     *   For grayscale or monochrome displays, the value is
     *   automatically converted to the proper range.
     *   You can also use the constants <c>FG_INK</c> to use the
     *   default drawing colour, <c>BG_INK</c> to use the default
     *   background colour, and <c>NO_INK</c> to disable outline drawing.
     * </para>
     * </summary>
     * <param name="color">
     *   the desired drawing color, as a 24-bit RGB value,
     *   or one of the constants <c>NO_INK</c>, <c>FG_INK</c>
     *   or <c>BG_INK</c>
     * </param>
     * <returns>
     *   <c>YAPI.SUCCESS</c> if the call succeeds.
     * </returns>
     * <para>
     *   On failure, throws an exception or returns a negative error code.
     * </para>
     */
    public virtual async Task<int> selectLineColor(int color)
    {
        int r;
        int g;
        int b;
        if (color==-1) {
            return await this.command_push("l_");
        }
        if (color==-2) {
            return await this.command_push("l-");
        }
        if (color==-3) {
            return await this.command_push("l*");
        }
        r = ((color >> 20) & 15);
        g = ((color >> 12) & 15);
        b = ((color >> 4) & 15);
        return await this.command_push("l"+String.Format("{0:x}",r)+""+String.Format("{0:x}",g)+""+String.Format("{0:x}",b));
    }

    /**
     * <summary>
     *   Selects the line width for drawing the outline of rectangular
     *   bars, discs and polygons, as well as for drawing lines.
     * <para>
     * </para>
     * </summary>
     * <param name="width">
     *   the desired line width, in pixels
     * </param>
     * <returns>
     *   <c>YAPI.SUCCESS</c> if the call succeeds.
     * </returns>
     * <para>
     *   On failure, throws an exception or returns a negative error code.
     * </para>
     */
    public virtual async Task<int> selectLineWidth(int width)
    {
        return await this.command_push("t"+Convert.ToString(width));
    }

    public virtual async Task<int> setAntialiasingMode(bool mode)
    {
        return await this.command_push("a"+(mode?"1":"0"));
    }

    /**
     * <summary>
     *   Draws a single pixel at the specified position.
     * <para>
     * </para>
     * </summary>
     * <param name="x">
     *   the distance from left of layer, in pixels
     * </param>
     * <param name="y">
     *   the distance from top of layer, in pixels
     * </param>
     * <returns>
     *   <c>YAPI.SUCCESS</c> if the call succeeds.
     * </returns>
     * <para>
     *   On failure, throws an exception or returns a negative error code.
     * </para>
     */
    public virtual async Task<int> drawPixel(int x,int y)
    {
        return await this.command_flush("P"+Convert.ToString(x)+","+Convert.ToString(y));
    }

    /**
     * <summary>
     *   Draws an empty rectangle at a specified position.
     * <para>
     * </para>
     * </summary>
     * <param name="x1">
     *   the distance from left of layer to the left border of the rectangle, in pixels
     * </param>
     * <param name="y1">
     *   the distance from top of layer to the top border of the rectangle, in pixels
     * </param>
     * <param name="x2">
     *   the distance from left of layer to the right border of the rectangle, in pixels
     * </param>
     * <param name="y2">
     *   the distance from top of layer to the bottom border of the rectangle, in pixels
     * </param>
     * <returns>
     *   <c>YAPI.SUCCESS</c> if the call succeeds.
     * </returns>
     * <para>
     *   On failure, throws an exception or returns a negative error code.
     * </para>
     */
    public virtual async Task<int> drawRect(int x1,int y1,int x2,int y2)
    {
        return await this.command_flush("R"+Convert.ToString(x1)+","+Convert.ToString(y1)+","+Convert.ToString(x2)+","+Convert.ToString(y2));
    }

    /**
     * <summary>
     *   Draws a filled rectangular bar at a specified position.
     * <para>
     * </para>
     * </summary>
     * <param name="x1">
     *   the distance from left of layer to the left border of the rectangle, in pixels
     * </param>
     * <param name="y1">
     *   the distance from top of layer to the top border of the rectangle, in pixels
     * </param>
     * <param name="x2">
     *   the distance from left of layer to the right border of the rectangle, in pixels
     * </param>
     * <param name="y2">
     *   the distance from top of layer to the bottom border of the rectangle, in pixels
     * </param>
     * <returns>
     *   <c>YAPI.SUCCESS</c> if the call succeeds.
     * </returns>
     * <para>
     *   On failure, throws an exception or returns a negative error code.
     * </para>
     */
    public virtual async Task<int> drawBar(int x1,int y1,int x2,int y2)
    {
        return await this.command_flush("B"+Convert.ToString(x1)+","+Convert.ToString(y1)+","+Convert.ToString(x2)+","+Convert.ToString(y2));
    }

    /**
     * <summary>
     *   Draws an empty circle at a specified position.
     * <para>
     * </para>
     * </summary>
     * <param name="x">
     *   the distance from left of layer to the center of the circle, in pixels
     * </param>
     * <param name="y">
     *   the distance from top of layer to the center of the circle, in pixels
     * </param>
     * <param name="r">
     *   the radius of the circle, in pixels
     * </param>
     * <returns>
     *   <c>YAPI.SUCCESS</c> if the call succeeds.
     * </returns>
     * <para>
     *   On failure, throws an exception or returns a negative error code.
     * </para>
     */
    public virtual async Task<int> drawCircle(int x,int y,int r)
    {
        return await this.command_flush("C"+Convert.ToString(x)+","+Convert.ToString(y)+","+Convert.ToString(r));
    }

    /**
     * <summary>
     *   Draws a filled disc at a given position.
     * <para>
     * </para>
     * </summary>
     * <param name="x">
     *   the distance from left of layer to the center of the disc, in pixels
     * </param>
     * <param name="y">
     *   the distance from top of layer to the center of the disc, in pixels
     * </param>
     * <param name="r">
     *   the radius of the disc, in pixels
     * </param>
     * <returns>
     *   <c>YAPI.SUCCESS</c> if the call succeeds.
     * </returns>
     * <para>
     *   On failure, throws an exception or returns a negative error code.
     * </para>
     */
    public virtual async Task<int> drawDisc(int x,int y,int r)
    {
        return await this.command_flush("D"+Convert.ToString(x)+","+Convert.ToString(y)+","+Convert.ToString(r));
    }

    /**
     * <summary>
     *   Selects a font to use for the next text drawing functions, by providing the name of the
     *   font file.
     * <para>
     *   You can use a built-in font as well as a font file that you have previously
     *   uploaded to the device built-in memory. If you experience problems selecting a font
     *   file, check the device logs for any error message such as missing font file or bad font
     *   file format.
     * </para>
     * </summary>
     * <param name="fontname">
     *   the font file name, embedded fonts are 8x8.yfm, Small.yfm, Medium.yfm, Large.yfm (not available on
     *   Yocto-MiniDisplay).
     * </param>
     * <returns>
     *   <c>YAPI.SUCCESS</c> if the call succeeds.
     * </returns>
     * <para>
     *   On failure, throws an exception or returns a negative error code.
     * </para>
     */
    public virtual async Task<int> selectFont(string fontname)
    {
        return await this.command_push("&"+fontname+""+((char)(27)).ToString());
    }

    /**
     * <summary>
     *   Draws a text string at the specified position.
     * <para>
     *   The point of the text that is aligned
     *   to the specified pixel position is called the anchor point, and can be chosen among
     *   several options. Text is rendered from left to right, without implicit wrapping.
     * </para>
     * </summary>
     * <param name="x">
     *   the distance from left of layer to the text anchor point, in pixels
     * </param>
     * <param name="y">
     *   the distance from top of layer to the text anchor point, in pixels
     * </param>
     * <param name="anchor">
     *   the text anchor point, chosen among the <c>YDisplayLayer.ALIGN</c> enumeration:
     *   <c>YDisplayLayer.ALIGN.TOP_LEFT</c>,         <c>YDisplayLayer.ALIGN.CENTER_LEFT</c>,
     *   <c>YDisplayLayer.ALIGN.BASELINE_LEFT</c>,    <c>YDisplayLayer.ALIGN.BOTTOM_LEFT</c>,
     *   <c>YDisplayLayer.ALIGN.TOP_CENTER</c>,       <c>YDisplayLayer.ALIGN.CENTER</c>,
     *   <c>YDisplayLayer.ALIGN.BASELINE_CENTER</c>,  <c>YDisplayLayer.ALIGN.BOTTOM_CENTER</c>,
     *   <c>YDisplayLayer.ALIGN.TOP_DECIMAL</c>,      <c>YDisplayLayer.ALIGN.CENTER_DECIMAL</c>,
     *   <c>YDisplayLayer.ALIGN.BASELINE_DECIMAL</c>, <c>YDisplayLayer.ALIGN.BOTTOM_DECIMAL</c>,
     *   <c>YDisplayLayer.ALIGN.TOP_RIGHT</c>,        <c>YDisplayLayer.ALIGN.CENTER_RIGHT</c>,
     *   <c>YDisplayLayer.ALIGN.BASELINE_RIGHT</c>,   <c>YDisplayLayer.ALIGN.BOTTOM_RIGHT</c>.
     * </param>
     * <param name="text">
     *   the text string to draw
     * </param>
     * <returns>
     *   <c>YAPI.SUCCESS</c> if the call succeeds.
     * </returns>
     * <para>
     *   On failure, throws an exception or returns a negative error code.
     * </para>
     */
    public virtual async Task<int> drawText(int x,int y,ALIGN anchor,string text)
    {
        return await this.command_flush("T"+Convert.ToString(x)+","+Convert.ToString(y)+","+((int)(anchor)).ToString()+","+text+""+((char)(27)).ToString());
    }

    /**
     * <summary>
     *   Draws an image previously uploaded to the device filesystem, at the specified position.
     * <para>
     *   At present time, GIF images are the only supported image format. If you experience
     *   problems using an image file, check the device logs for any error message such as
     *   missing image file or bad image file format.
     * </para>
     * </summary>
     * <param name="x">
     *   the distance from left of layer to the left of the image, in pixels
     * </param>
     * <param name="y">
     *   the distance from top of layer to the top of the image, in pixels
     * </param>
     * <param name="imagename">
     *   the GIF file name
     * </param>
     * <returns>
     *   <c>YAPI.SUCCESS</c> if the call succeeds.
     * </returns>
     * <para>
     *   On failure, throws an exception or returns a negative error code.
     * </para>
     */
    public virtual async Task<int> drawImage(int x,int y,string imagename)
    {
        return await this.command_flush("*"+Convert.ToString(x)+","+Convert.ToString(y)+","+imagename+""+((char)(27)).ToString());
    }

    /**
     * <summary>
     *   Draws a bitmap at the specified position.
     * <para>
     *   The bitmap is provided as a binary object,
     *   where each pixel maps to a bit, from left to right and from top to bottom.
     *   The most significant bit of each byte maps to the leftmost pixel, and the least
     *   significant bit maps to the rightmost pixel. Bits set to 1 are drawn using the
     *   layer selected pen color. Bits set to 0 are drawn using the specified background
     *   gray level, unless -1 is specified, in which case they are not drawn at all
     *   (as if transparent).
     * </para>
     * </summary>
     * <param name="x">
     *   the distance from left of layer to the left of the bitmap, in pixels
     * </param>
     * <param name="y">
     *   the distance from top of layer to the top of the bitmap, in pixels
     * </param>
     * <param name="w">
     *   the width of the bitmap, in pixels
     * </param>
     * <param name="bitmap">
     *   a binary object
     * </param>
     * <param name="bgcol">
     *   the background gray level to use for zero bits (0 = black,
     *   255 = white), or -1 to leave the pixels unchanged
     * </param>
     * <returns>
     *   <c>YAPI.SUCCESS</c> if the call succeeds.
     * </returns>
     * <para>
     *   On failure, throws an exception or returns a negative error code.
     * </para>
     */
    public virtual async Task<int> drawBitmap(int x,int y,int w,byte[] bitmap,int bgcol)
    {
        string destname;
        destname = "layer"+Convert.ToString(_id)+":"+Convert.ToString(w)+","+Convert.ToString(bgcol)+"@"+Convert.ToString(x)+","+Convert.ToString(y);
        return await _display.upload(destname, bitmap);
    }

    /**
     * <summary>
     *   Draws a GIF image provided as a binary buffer at the specified position.
     * <para>
     *   If the image drawing must be included in an animation sequence, save it
     *   in the device filesystem first and use <c>drawImage</c> instead.
     * </para>
     * </summary>
     * <param name="x">
     *   the distance from left of layer to the left of the image, in pixels
     * </param>
     * <param name="y">
     *   the distance from top of layer to the top of the image, in pixels
     * </param>
     * <param name="gifimage">
     *   a binary object with the content of a GIF file
     * </param>
     * <returns>
     *   <c>YAPI.SUCCESS</c> if the call succeeds.
     * </returns>
     * <para>
     *   On failure, throws an exception or returns a negative error code.
     * </para>
     */
    public virtual async Task<int> drawGIF(int x,int y,byte[] gifimage)
    {
        string destname;
        destname = "layer"+Convert.ToString(_id)+":G,-1@"+Convert.ToString(x)+","+Convert.ToString(y);
        return await _display.upload(destname, gifimage);
    }

    /**
     * <summary>
     *   Moves the drawing pointer of this layer to the specified position.
     * <para>
     * </para>
     * </summary>
     * <param name="x">
     *   the distance from left of layer, in pixels
     * </param>
     * <param name="y">
     *   the distance from top of layer, in pixels
     * </param>
     * <returns>
     *   <c>YAPI.SUCCESS</c> if the call succeeds.
     * </returns>
     * <para>
     *   On failure, throws an exception or returns a negative error code.
     * </para>
     */
    public virtual async Task<int> moveTo(int x,int y)
    {
        return await this.command_push("@"+Convert.ToString(x)+","+Convert.ToString(y));
    }

    /**
     * <summary>
     *   Draws a line from current drawing pointer position to the specified position.
     * <para>
     *   The specified destination pixel is included in the line. The pointer position
     *   is then moved to the end point of the line.
     * </para>
     * </summary>
     * <param name="x">
     *   the distance from left of layer to the end point of the line, in pixels
     * </param>
     * <param name="y">
     *   the distance from top of layer to the end point of the line, in pixels
     * </param>
     * <returns>
     *   <c>YAPI.SUCCESS</c> if the call succeeds.
     * </returns>
     * <para>
     *   On failure, throws an exception or returns a negative error code.
     * </para>
     */
    public virtual async Task<int> lineTo(int x,int y)
    {
        return await this.command_flush("-"+Convert.ToString(x)+","+Convert.ToString(y));
    }

    /**
     * <summary>
     *   Starts drawing a polygon with the first corner at the specified position.
     * <para>
     * </para>
     * </summary>
     * <param name="x">
     *   the distance from left of layer, in pixels
     * </param>
     * <param name="y">
     *   the distance from top of layer, in pixels
     * </param>
     * <returns>
     *   <c>YAPI.SUCCESS</c> if the call succeeds.
     * </returns>
     * <para>
     *   On failure, throws an exception or returns a negative error code.
     * </para>
     */
    public virtual async Task<int> polygonStart(int x,int y)
    {
        _polyPrevX = x;
        _polyPrevY = y;
        return await this.command_push("["+Convert.ToString(x)+","+Convert.ToString(y));
    }

    /**
     * <summary>
     *   Adds a point to the currently open polygon, previously opened using
     *   <c>polygonStart</c>.
     * <para>
     * </para>
     * </summary>
     * <param name="x">
     *   the distance from left of layer to the new point, in pixels
     * </param>
     * <param name="y">
     *   the distance from top of layer to the new point, in pixels
     * </param>
     * <returns>
     *   <c>YAPI.SUCCESS</c> if the call succeeds.
     * </returns>
     * <para>
     *   On failure, throws an exception or returns a negative error code.
     * </para>
     */
    public virtual async Task<int> polygonAdd(int x,int y)
    {
        int dx;
        int dy;
        dx = x - _polyPrevX;
        dy = y - _polyPrevY;
        _polyPrevX = x;
        _polyPrevY = y;
        return await this.command_flush(";"+Convert.ToString(dx)+","+Convert.ToString(dy));
    }

    /**
     * <summary>
     *   Close the currently open polygon, fill its content the fill color currently
     *   selected for the layer, and draw its outline using the selected line color.
     * <para>
     * </para>
     * </summary>
     * <returns>
     *   <c>YAPI.SUCCESS</c> if the call succeeds.
     * </returns>
     * <para>
     *   On failure, throws an exception or returns a negative error code.
     * </para>
     */
    public virtual async Task<int> polygonEnd()
    {
        return await this.command_flush("]");
    }

    /**
     * <summary>
     *   Outputs a message in the console area, and advances the console pointer accordingly.
     * <para>
     *   The console pointer position is automatically moved to the beginning
     *   of the next line when a newline character is met, or when the right margin
     *   is hit. When the new text to display extends below the lower margin, the
     *   console area is automatically scrolled up.
     * </para>
     * </summary>
     * <param name="text">
     *   the message to display
     * </param>
     * <returns>
     *   <c>YAPI.SUCCESS</c> if the call succeeds.
     * </returns>
     * <para>
     *   On failure, throws an exception or returns a negative error code.
     * </para>
     */
    public virtual async Task<int> consoleOut(string text)
    {
        return await this.command_flush("!"+text+""+((char)(27)).ToString());
    }

    /**
     * <summary>
     *   Sets up display margins for the <c>consoleOut</c> function.
     * <para>
     * </para>
     * </summary>
     * <param name="x1">
     *   the distance from left of layer to the left margin, in pixels
     * </param>
     * <param name="y1">
     *   the distance from top of layer to the top margin, in pixels
     * </param>
     * <param name="x2">
     *   the distance from left of layer to the right margin, in pixels
     * </param>
     * <param name="y2">
     *   the distance from top of layer to the bottom margin, in pixels
     * </param>
     * <returns>
     *   <c>YAPI.SUCCESS</c> if the call succeeds.
     * </returns>
     * <para>
     *   On failure, throws an exception or returns a negative error code.
     * </para>
     */
    public virtual async Task<int> setConsoleMargins(int x1,int y1,int x2,int y2)
    {
        return await this.command_push("m"+Convert.ToString(x1)+","+Convert.ToString(y1)+","+Convert.ToString(x2)+","+Convert.ToString(y2));
    }

    /**
     * <summary>
     *   Sets up the background color used by the <c>clearConsole</c> function and by
     *   the console scrolling feature.
     * <para>
     * </para>
     * </summary>
     * <param name="bgcol">
     *   the background gray level to use when scrolling (0 = black,
     *   255 = white), or -1 for transparent
     * </param>
     * <returns>
     *   <c>YAPI.SUCCESS</c> if the call succeeds.
     * </returns>
     * <para>
     *   On failure, throws an exception or returns a negative error code.
     * </para>
     */
    public virtual async Task<int> setConsoleBackground(int bgcol)
    {
        return await this.command_push("b"+Convert.ToString(bgcol));
    }

    /**
     * <summary>
     *   Sets up the wrapping behavior used by the <c>consoleOut</c> function.
     * <para>
     * </para>
     * </summary>
     * <param name="wordwrap">
     *   <c>true</c> to wrap only between words,
     *   <c>false</c> to wrap on the last column anyway.
     * </param>
     * <returns>
     *   <c>YAPI.SUCCESS</c> if the call succeeds.
     * </returns>
     * <para>
     *   On failure, throws an exception or returns a negative error code.
     * </para>
     */
    public virtual async Task<int> setConsoleWordWrap(bool wordwrap)
    {
        return await this.command_push("w"+(wordwrap?"1":"0"));
    }

    /**
     * <summary>
     *   Blanks the console area within console margins, and resets the console pointer
     *   to the upper left corner of the console.
     * <para>
     * </para>
     * </summary>
     * <returns>
     *   <c>YAPI.SUCCESS</c> if the call succeeds.
     * </returns>
     * <para>
     *   On failure, throws an exception or returns a negative error code.
     * </para>
     */
    public virtual async Task<int> clearConsole()
    {
        return await this.command_flush("^");
    }

    /**
     * <summary>
     *   Sets the position of the layer relative to the display upper left corner.
     * <para>
     *   When smooth scrolling is used, the display offset of the layer is
     *   automatically updated during the next milliseconds to animate the move of the layer.
     * </para>
     * </summary>
     * <param name="x">
     *   the distance from left of display to the upper left corner of the layer
     * </param>
     * <param name="y">
     *   the distance from top of display to the upper left corner of the layer
     * </param>
     * <param name="scrollTime">
     *   number of milliseconds to use for smooth scrolling, or
     *   0 if the scrolling should be immediate.
     * </param>
     * <returns>
     *   <c>YAPI.SUCCESS</c> if the call succeeds.
     * </returns>
     * <para>
     *   On failure, throws an exception or returns a negative error code.
     * </para>
     */
    public virtual async Task<int> setLayerPosition(int x,int y,int scrollTime)
    {
        return await this.command_flush("#"+Convert.ToString(x)+","+Convert.ToString(y)+","+Convert.ToString(scrollTime));
    }

    /**
     * <summary>
     *   Hides the layer.
     * <para>
     *   The state of the layer is preserved but the layer is not displayed
     *   on the screen until the next call to <c>unhide()</c>. Hiding the layer can positively
     *   affect the drawing speed, since it postpones the rendering until all operations are
     *   completed (double-buffering).
     * </para>
     * </summary>
     * <returns>
     *   <c>YAPI.SUCCESS</c> if the call succeeds.
     * </returns>
     * <para>
     *   On failure, throws an exception or returns a negative error code.
     * </para>
     */
    public virtual async Task<int> hide()
    {
        await this.command_push("h");
        _hidden = true;
        return await this.flush_now();
    }

    /**
     * <summary>
     *   Shows the layer.
     * <para>
     *   Shows the layer again after a hide command.
     * </para>
     * </summary>
     * <returns>
     *   <c>YAPI.SUCCESS</c> if the call succeeds.
     * </returns>
     * <para>
     *   On failure, throws an exception or returns a negative error code.
     * </para>
     */
    public virtual async Task<int> unhide()
    {
        _hidden = false;
        return await this.command_flush("s");
    }

    /**
     * <summary>
     *   Gets parent YDisplay.
     * <para>
     *   Returns the parent YDisplay object of the current YDisplayLayer.
     * </para>
     * </summary>
     * <returns>
     *   an <c>YDisplay</c> object
     * </returns>
     */
    public virtual async Task<YDisplay> get_display()
    {
        return _display;
    }

    /**
     * <summary>
     *   Returns the display width, in pixels.
     * <para>
     * </para>
     * </summary>
     * <returns>
     *   an integer corresponding to the display width, in pixels
     * </returns>
     * <para>
     *   On failure, throws an exception or returns YDisplayLayer.DISPLAYWIDTH_INVALID.
     * </para>
     */
    public virtual async Task<int> get_displayWidth()
    {
        return await _display.get_displayWidth();
    }

    /**
     * <summary>
     *   Returns the display height, in pixels.
     * <para>
     * </para>
     * </summary>
     * <returns>
     *   an integer corresponding to the display height, in pixels
     * </returns>
     * <para>
     *   On failure, throws an exception or returns YDisplayLayer.DISPLAYHEIGHT_INVALID.
     * </para>
     */
    public virtual async Task<int> get_displayHeight()
    {
        return await _display.get_displayHeight();
    }

    /**
     * <summary>
     *   Returns the width of the layers to draw on, in pixels.
     * <para>
     * </para>
     * </summary>
     * <returns>
     *   an integer corresponding to the width of the layers to draw on, in pixels
     * </returns>
     * <para>
     *   On failure, throws an exception or returns YDisplayLayer.LAYERWIDTH_INVALID.
     * </para>
     */
    public virtual async Task<int> get_layerWidth()
    {
        return await _display.get_layerWidth();
    }

    /**
     * <summary>
     *   Returns the height of the layers to draw on, in pixels.
     * <para>
     * </para>
     * </summary>
     * <returns>
     *   an integer corresponding to the height of the layers to draw on, in pixels
     * </returns>
     * <para>
     *   On failure, throws an exception or returns YDisplayLayer.LAYERHEIGHT_INVALID.
     * </para>
     */
    public virtual async Task<int> get_layerHeight()
    {
        return await _display.get_layerHeight();
    }

#pragma warning restore 1998
    //--- (end of generated code: YDisplayLayer implementation)
    }

}