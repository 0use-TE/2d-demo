### HP设计
HpManager会挂载一个全局，所有敌人进入屏幕，和退出，包括受伤，都会发布事件
可见CharacterBase,但是由于BuildingBase继承的是StaticBody，所以不能共享这部分逻辑
建筑需要单独写一遍这个逻辑...  

决定了VisibleOnScreenNotifier2D层单独建一个脚本，通过结构DI获取到Entity
然后，处理这部分逻辑，实现了统一性  

然后发布这些事件，HPManager订阅，利用对象池高效处理，动态显示血条在实体头上，
或者移除头上的血条。