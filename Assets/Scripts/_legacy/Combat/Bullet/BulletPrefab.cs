using Scorewarrior.Test.Models;
using Scorewarrior.Test.Services;
using UnityEngine;

namespace Scorewarrior.Test.Views
{
	public sealed class BulletPrefab : CachedMonoBehaviour, IBulletPrefab
    {
        [Min(1f)]public float moveSpeed = 30f;

        IDamageable _target;
		float _damage;
		bool _hit;
        Transform _transform;
		Vector3 _startedPosition;
		Vector3 _moveDirection;
		float _totalDistance;
		float _currentDistance;
        
        // ReSharper disable Unity.PerformanceAnalysis
        void IBulletPrefab.Init(float damage, IDamageable target, Vector3 targetPosition, bool hit)
		{
            _damage = damage;
            _target = target;
			_hit = hit;
            
            _transform = transform;
			_startedPosition = _transform.position;

            _moveDirection = Vector3.Normalize(targetPosition - _startedPosition);
            _totalDistance = Vector3.Distance(targetPosition, _startedPosition);
            _currentDistance = 0f;
        }

        public override void OnUpdate(float deltaTime)
        {
            _currentDistance += deltaTime * moveSpeed;
            
            if (_currentDistance < _totalDistance)
            {
                _transform.position = _startedPosition + _currentDistance * _moveDirection;
            }
            else
            {
                if (_hit)
                {
                    _target.TakeDamage(_damage);
                }
                
                GameObjectPool.Return(gameObject);
            }
        }
	}
}