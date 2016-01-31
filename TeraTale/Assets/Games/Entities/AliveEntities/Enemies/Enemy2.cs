using UnityEngine;
using System.Collections;

namespace sunho
{
    [RequireComponent(typeof(SphereCollider))]
    public abstract class Enemy2 : AliveEntity
    {
        #region Variable
        protected GameObject _player;
        protected NavMeshAgent _nav;
        protected Animator _animator;
        protected bool _dirChk = false;
        protected bool _chaseChk = false;
        protected Vector3 _playerpos;
        protected Vector3 _distance;
        protected Vector3 _prePos;
        public float _fAttackDistance;
        public float _fView;
        #endregion

        new IEnumerator Start()
        {
            base.Start();
            while (Player.mine == null)
                yield return null;
            _player = Player.mine.gameObject;
            _nav = GetComponent<NavMeshAgent>();
            _animator = GetComponent<Animator>();
        }

        protected new void Update()
        {
            base.Update();
            if (_player == null)
                return;
            _distance = _player.transform.localPosition - this.transform.localPosition;

            _playerpos = _player.transform.position;

            this.transform.LookAt(_player.transform.position);

            if (_chaseChk)
            {
                if (ChasePlayer())
                    OffNavi();
                else
                    OnNavi(_playerpos);
            }
        }
        #region ChasePlayer
        protected bool ChasePlayer()
        {
            if (_distance.magnitude <= _fAttackDistance)
                return true;
            else
                return false;
        }
        #endregion

        #region VirtualFunc
        protected abstract void OnNavi(Vector3 pos);

        protected abstract void OffNavi();

        protected abstract void Stay();
        #endregion

        #region Trigger
        void OnTriggerEnter(Collider col)
        {
            _prePos = transform.position;
            if (col.transform.tag == "Player")
            {
                _chaseChk = true;
            }
        }

        void OnTriggerExit()
        {
            Stay();
            _chaseChk = false;
        }
        #endregion
    }
}