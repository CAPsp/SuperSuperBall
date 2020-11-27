
namespace ssb
{
    // ステート管理を担うクラス
    public class StateMachine
    {
        #region プロパティ

        // 現在のステート
        public BaseState _CurrentState { private set; get; }

        // １つ前のステート
        public BaseState _PrevState { private set; get; } = null;

        #endregion // プロパティ

        #region フィールド

        // このステートマシンを所有するオブジェクト
        private CharaBaseBehaviour _OwnerCharaBehaviour;

        #endregion // フィールド

        #region 公開メソッド

        // コンストラクタで初期ステートを決める
        public StateMachine(BaseState state, CharaBaseBehaviour owner)
        {
            _CurrentState           = state;
            _OwnerCharaBehaviour    = owner;
        }

        // FSM更新時に呼ぶ
        public void update()
        {
            _CurrentState.update();
        }

        // 新しいステートに変更する
        public void changeState(BaseState state)
        {
            // ステートに変更がない場合は処理を中断
            if(_CurrentState.GetType() == state.GetType())
            {
                return;
            }

            _PrevState = _CurrentState;

            _CurrentState.exit();

            _CurrentState = state;

            _CurrentState.enter();
        }

        // １つ前のステートに戻る
        public void revertToPrevState()
        {
            if(_PrevState != null)
            {
                changeState(_PrevState);
            }
        }

        #endregion // 公開メソッド

    }
}