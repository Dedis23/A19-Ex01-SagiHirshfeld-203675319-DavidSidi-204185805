using Microsoft.Xna.Framework;

namespace Infrastructure.ServiceInterfaces
{
    public interface IAnimated
    {

    }
    public interface ICellAnimated : IAnimated
    {
        float FrameTime { get; set; }
        int FrameIndex { get; set; }
        int NumOfFrames { get; set; }
        Rectangle SourceRectangle { get; set; }
    }
    interface IAnimationManager
    {
        void AddObjectToMonitor(IAnimated i_Animated);
    }
}