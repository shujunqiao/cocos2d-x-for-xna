/****************************************************************************
Copyright (c) 2010-2012 cocos2d-x.org
Copyright (c) 2008-2010 Ricardo Quesada
Copyright (c) 2011      Zynga Inc.
Copyright (c) 2011-2012 openxlive.com
 
Permission is hereby granted, free of charge, to any person obtaining a copy
of this software and associated documentation files (the "Software"), to deal
in the Software without restriction, including without limitation the rights
to use, copy, modify, merge, publish, distribute, sublicense, and/or sell
copies of the Software, and to permit persons to whom the Software is
furnished to do so, subject to the following conditions:

The above copyright notice and this permission notice shall be included in
all copies or substantial portions of the Software.

THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS OR
IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF MERCHANTABILITY,
FITNESS FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT. IN NO EVENT SHALL THE
AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM, DAMAGES OR OTHER
LIABILITY, WHETHER IN AN ACTION OF CONTRACT, TORT OR OTHERWISE, ARISING FROM,
OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR OTHER DEALINGS IN
THE SOFTWARE.
****************************************************************************/

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace cocos2d
{
    /// <summary>
    /// CCTwirl action
    /// </summary>
    public class CCTwirl : CCGrid3DAction
    {
        /// <summary>
        /// get twirl center
        /// </summary>
        public CCPoint getPosition()
        {
            return m_position;
        }

        /// <summary>
        /// set twirl center
        /// </summary>
        public void setPosition(CCPoint position)
        {
            m_position = position;
            m_positionInPixels.x = position.x * CCDirector.sharedDirector().ContentScaleFactor;
            m_positionInPixels.y = position.y * CCDirector.sharedDirector().ContentScaleFactor;
        }

        public float getAmplitude()
        {
            return m_fAmplitude;
        }

        public void setAmplitude(float fAmplitude)
        {
            m_fAmplitude = fAmplitude;
        }

        public float getAmplitudeRate()
        {
            return m_fAmplitudeRate;
        }

        public void setAmplitudeRate(float fAmplitudeRate)
        {
            m_fAmplitudeRate = fAmplitudeRate;
        }

        /// <summary>
        /// initializes the action with center position, number of twirls, amplitude, a grid size and duration
        /// </summary>
        public bool initWithPosition(CCPoint pos, int t, float amp, ccGridSize gridSize,
            float duration)
        {
            if (base.initWithSize(gridSize, duration))
            {
                m_positionInPixels = new CCPoint();
                setPosition(pos);
                m_nTwirls = t;
                m_fAmplitude = amp;
                m_fAmplitudeRate = 1.0f;

                return true;
            }

            return false;
        }

        public override CCObject copyWithZone(CCZone pZone)
        {
            CCZone pNewZone = null;
            CCTwirl pCopy = null;
            if (pZone != null && pZone.m_pCopyObject != null)
            {
                //in case of being called at sub class
                pCopy = (CCTwirl)(pZone.m_pCopyObject);
            }
            else
            {
                pCopy = new CCTwirl();
                pZone = pNewZone = new CCZone(pCopy);
            }

            base.copyWithZone(pZone);

            pCopy.initWithPosition(m_position, m_nTwirls, m_fAmplitude, m_sGridSize, m_fDuration);
            return pCopy;
        }

        public override void update(float time)
        {
            int i, j;
            CCPoint c = m_positionInPixels;

            for (i = 0; i < (m_sGridSize.x + 1); ++i)
            {
                for (j = 0; j < (m_sGridSize.y + 1); ++j)
                {
                    ccVertex3F v = originalVertex(new ccGridSize(i, j));

                    CCPoint avg = new CCPoint(i - (m_sGridSize.x / 2.0f), j - (m_sGridSize.y / 2.0f));
                    float r = (float)Math.Sqrt((avg.x * avg.x + avg.y * avg.y));

                    float amp = 0.1f * m_fAmplitude * m_fAmplitudeRate;
                    float a = r * (float)Math.Cos((float)Math.PI / 2.0f + time * (float)Math.PI * m_nTwirls * 2) * amp;

                    CCPoint d = new CCPoint();

                    d.x = (float)Math.Sin(a) * (v.y - c.y) + (float)Math.Cos(a) * (v.x - c.x);
                    d.y = (float)Math.Cos(a) * (v.y - c.y) - (float)Math.Sin(a) * (v.x - c.x);

                    v.x = c.x + d.x;
                    v.y = c.y + d.y;

                    setVertex(new ccGridSize(i, j), v);
                }
            }
        }


        /// <summary>
        ///  creates the action with center position, number of twirls, amplitude, a grid size and duration
        /// </summary>
        public static CCTwirl actionWithPosition(CCPoint pos, int t, float amp, ccGridSize gridSize,
            float duration)
        {
            CCTwirl pAction = new CCTwirl();

            if (pAction.initWithPosition(pos, t, amp, gridSize, duration))
            {
                return pAction;
            }

            return null;
        }

        /* twirl center */
        protected CCPoint m_position;
        protected int m_nTwirls;
        protected float m_fAmplitude;
        protected float m_fAmplitudeRate;

        /*@since v0.99.5 */
        protected CCPoint m_positionInPixels;
    }
}
