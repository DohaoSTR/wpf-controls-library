using System;

namespace ZdfFlatUI.BaseControl
{
    public interface IUIElement : IDisposable
    {
        /// <summary>
        /// 注册事件
        /// </summary>
        void EventsRegistion();

        /// <summary>
        /// 解除事件注册
        /// </summary>
        void EventDeregistration();
    }
}
