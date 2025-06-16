using UnityEngine;

public class CannonTower : Tower //вообще можно использовать композицию
{
	[Header("Cannon")]
	[SerializeField] Transform turret; //rot Y
	[SerializeField] Transform cannon; //rot X
	[SerializeField] Transform aim;
	[SerializeField] float rotSpeed = 1.0f;


    private void Awake()
    {
		projectile = myInfo.projectilePrefab.GetComponent<BaseProjectile>();
    }

	protected override void Shot()
	{
		base.Shot();

		currentProjectile.transform.position = shootPoint.position;
		currentProjectile.transform.rotation = shootPoint.rotation;
	}

	



	Vector3 rot;
	Vector3 dir;
	Vector3 v;
	Quaternion rotGoal;
	float destY;
	protected override void FollowTarget()
	{
		if (currentTarget)
		{
			v = FindTargetDest3(); //оптимизнуть таким образом, чтобы
								  //1) лерпать плавно от цели к цели - OK
								  //2) считать позицию выстрела за секунду до выстрела

			canShot = false; //не стреляем зря если не довернули

			//if (t * Va_ > range) //проверка если упреждение за дальностью выстрела, то не стреляем (опционально)
			//	return;

			//smooth
			aim.LookAt(v);//currentTarget.transform);
			dir = (v - turret.position).normalized;
			rotGoal = Quaternion.LookRotation(dir);
			turret.rotation = Quaternion.RotateTowards(turret.rotation, rotGoal, rotSpeed * Time.deltaTime);
			rot = turret.localEulerAngles;
			rot.x = 0;
			turret.localEulerAngles = rot;
			if (Mathf.Abs(aim.localEulerAngles.y - turret.localEulerAngles.y) <= 2.0f)
			{
				rot = aim.localEulerAngles;
				rot.x = 0;
				turret.localEulerAngles = rot;
				canShot = true;
			}

			/* no smooth
			turret.LookAt(v);//currentTarget.transform);
			rot = turret.localEulerAngles;
			rot.x = 0;
			rot.z = 0;
			turret.localEulerAngles = rot;
			*/

			cannon.LookAt(v);// currentTarget.transform);
			rot = cannon.localEulerAngles; //это можно ограничить через Кламп если пушка наклоняется слишком низко - ТЗ
			rot.y = 0;
			rot.z = 0;
			cannon.localEulerAngles = rot; 
		}
	}












	float t0;
	float t;
	float i;

	Vector3 A;
	Vector3 B;
	float Va_;
	Vector3 Vb;

	Vector3 X0;
	Vector3 X1;

	//второй способ через В+(В-Х1)/2 в итерации

	//поидее это упрощенный метод прогноза коррекции
	//но я не стал считать диф.уравнения
	Vector3 FindTargetDest()
	{
		A = shootPoint.position;
		B = currentTarget.transform.position;
		Va_ = projectile.Projectile.MyInfo.speed; 
		Vb = currentTarget.GetTranslationVector() * currentTarget.GetSpeed();

		X0 = B;
		t0 = Vector3.Distance(A, X0) / Va_;
		X1 = B + Vb * t0;
		t = Vector3.Distance(A, X1) / Va_;
		//t - время снаряда до Х1
		//т0 - время противника до Х1
		//если т0 больше т, то стреляем ближе, если т0 меньше т, то стреляем дальше
		Debug.DrawLine(A, X1, Color.black);

		for (i = 0; i < 10 && Mathf.Abs(t - t0) > 0.05f; i++)
		{
			if (t0 > t)
			{
				X0 = X1;
				t0 = t;//Vector3.Distance(A, X0) / Va_;
				X1 = B + Vb * t0;
				t = Vector3.Distance(A, X1) / Va_;
				X1 = B + Vb * t;
			}
			if (t0 < t)//ок
			{
				X0 = X1;
				t0 = t;//Vector3.Distance(A, X0) / Va_;
				X1 = B + Vb * t0;
				t = Vector3.Distance(A, X1) / Va_;
			}
		}

		Debug.DrawLine(A, B, Color.green);
		Debug.DrawLine(A, X1, Color.red);

		return X1;
	}


	Vector3 FindTargetDest2()
    {
		A = shootPoint.position;
		B = currentTarget.transform.position;
		Va_ = projectile.Projectile.MyInfo.speed;
		Vb = currentTarget.GetTranslationVector() * currentTarget.GetSpeed();

		X0 = B;
		t0 = Vector3.Distance(A, X0) / Va_;
		X1 = B + Vb * t0;

		Debug.DrawLine(A, X1, Color.black);

		for (i = 0; i < 10 && Mathf.Abs(t - t0) > 0.05f; i++) // 
		{
			t0 = t;
			t = Vector3.Distance(A, X0 + (X0 - X1) / 2.0f) / Va_;
			X1 = B + Vb * t;
		}



		Debug.DrawLine(A, B, Color.green);
		Debug.DrawLine(A, X1, Color.red);

		return X1;
	}



	private Vector3 FindTargetDest3()
	{
		Vector3 shooterPos = shootPoint.position;
		Vector3 targetPos = currentTarget.transform.position;
		Vector3 targetVelocity = currentTarget.GetTranslationVector() * currentTarget.GetSpeed();
		float projectileSpeed = projectile.Projectile.MyInfo.speed;

		// Квадратное уравнение для времени полёта снаряда
		float a = Vector3.Dot(targetVelocity, targetVelocity) - projectileSpeed * projectileSpeed;
		float b = 2 * Vector3.Dot(targetVelocity, targetPos - shooterPos);
		float c = Vector3.Dot(targetPos - shooterPos, targetPos - shooterPos);
		float D = b * b - 4 * a * c;

		if (D < 0) return targetPos; // Нет решения, стреляем в текущую позицию

		float t = (-b - Mathf.Sqrt(D)) / (2 * a); // Меньший корень
		if (t < 0) t = (-b + Mathf.Sqrt(D)) / (2 * a); // Если отрицательный, берём другой корень
		if (t < 0) return targetPos; // Если всё ещё отрицательный, стреляем в текущую позицию

		return targetPos + targetVelocity * t;
	}


}

