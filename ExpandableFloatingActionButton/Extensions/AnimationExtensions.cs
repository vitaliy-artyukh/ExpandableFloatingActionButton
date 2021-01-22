using System;
using System.Threading.Tasks;
using Xamarin.Forms;

namespace ExpandableFloatingActionButton.Extensions
{
    public static class AnimationExtensions
    {
        public static Task<bool> AnimatePropertyAsync(this View view, Action<double, View> animationAction, double from, double to, Easing easing = null, uint length = 250)
        {
            var taskCompletionSource = new TaskCompletionSource<bool>();
            var animation = new Animation(value => animationAction?.Invoke(value, view), from, to, easing);
            animation.Commit(view, nameof(AnimatePropertyAsync), length: length, easing: easing, finished: (value, result) => taskCompletionSource.TrySetResult(result));

            return taskCompletionSource.Task;
        }
    }
}