using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AndroidLib.Results;

namespace AndroidLib.Interaction
{
    public class ScreenMirror
    {
        private Device mDevice;

        internal ScreenMirror(Device dev)
        {
            mDevice = dev;
        }

        /// <summary>
        /// Performs a screenshot and transfers the screenshot to the computer and reads it into a <see cref="Image"/>. The temporary files will be deleted at the end of this method
        /// </summary>
        /// <returns>The image</returns>
        public InteractionResult<Image> CopyFromScreen()
        {
            string savePath = CopyFromScreenNoPull();
            string v = Path.Combine(ResourceManager.tempPath, "CopyFromScreen.png");

            //Pull it 
            AdbPushPullResult pullResult = mDevice.Pull(savePath, v);
            if(pullResult.Success)
            {
                try
                {
                    //Read the file
                    Image result = Image.FromFile(v);

                    //Delete both files. On device and computer
                    mDevice.FileSystem.RemoveObject(savePath);
                    File.Delete(v);

                    //Return the Image
                    return new InteractionResult<Image>(result, true, null);
                }
                catch (Exception ex)
                {
                    return new InteractionResult<Image>(null, false, ex);
                }
            }
            else
            {
                return new InteractionResult<Image>(null, false, new Exception(pullResult.Error.ToString()));
            }
        }

        /// <summary>
        /// Performs a screenshot and doesn't pull the file to computer
        /// </summary>
        /// <returns>The path to the png file on the device</returns>
        public string CopyFromScreenNoPull()
        {
            //Save it to this path
            string savePath = "/sdcard/Screenshot made by AndroidLib.png";

            //Take the screenshot
            mDevice.CommandShell.Exec("screencap " + savePath);

            //Return filepath
            return savePath;
        }

        /// <summary>
        /// Record the screen to .mp4 and move it to the path you passed in the arguments
        /// </summary>
        /// <param name="savePath">The path to save the path to.</param>
        /// <param name="timeLimit">The time to record. Maximum is 180 seconds (=3 minutes).</param>
        /// <param name="bitrate">The bitrate of the video. For a bitrate of 5 Mbps set it to 5000000.</param>
        /// <param name="rotate">Do a 90 degree rotation.</param>
        /// <param name="videoSize">If you don't want to pass this use <see cref="Size.Empty"/></param>
        /// <returns>The path to the file on computer</returns>
        //public async InteractionResult<string> RecordFromScreen(string savePath, Size videoSize, int timeLimit = 180, int bitrate = 0, bool rotate = false)
        //{

        //}

        /// <summary>
        /// Record the screen to .mp4
        /// </summary>
        /// <param name="timeLimit">The time to record. Maximum is 180 seconds (=3 minutes).</param>
        /// <param name="bitrate">The bitrate of the video. For a bitrate of 5 Mbps set it to 5000000.</param>
        /// <param name="rotate">Do a 90 degree rotation.</param>
        /// <param name="videoSize">If you don't want to pass this use <see cref="Size.Empty"/></param>
        /// <returns>The path to the file on device</returns>
        //public async string RecordFromScreenNoPull(Size videoSize, int timeLimit = 180, int bitrate = 0, bool rotate = false)
        //{

        //}
    }
}
