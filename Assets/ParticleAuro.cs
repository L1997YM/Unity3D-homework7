using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ParticleAuro : MonoBehaviour {

	public class CirclePosition{
		public float radius,angle,time;
		public CirclePosition(float radius,float angle,float time){
			this.radius = radius;
			this.angle = angle;
			this.time = time;
		}
	}

	private ParticleSystem particles;
	private ParticleSystem.Particle[] particleArr;
	private CirclePosition[] circle;
	public int count;            //粒子数量
	public float size;           //粒子大小
	public float minradius;       //内圈半径
	public float maxradius;      //外圈半径
	public bool clockwise;        //转向
	public Gradient colorGradient;

	void Start() {
		particleArr = new ParticleSystem.Particle[count];
		circle = new CirclePosition[count];
		//设置粒子系统各项数值
		particles = this.GetComponent<ParticleSystem>();
		particles.startSize = size;
		particles.maxParticles = count;
		particles.Emit(count);              //发射粒子
		particles.GetParticles(particleArr);

		GradientAlphaKey[] alphaKeys = new GradientAlphaKey[5];
		alphaKeys[0].time = 0.0f; alphaKeys[0].alpha = 1.0f;
		alphaKeys[1].time = 0.4f; alphaKeys[1].alpha = 0.4f;
		alphaKeys[2].time = 0.6f; alphaKeys[2].alpha = 1.0f;
		alphaKeys[3].time = 0.9f; alphaKeys[3].alpha = 0.4f;
		alphaKeys[4].time = 1.0f; alphaKeys[4].alpha = 0.9f;
		GradientColorKey[] colorKeys = new GradientColorKey[2];
		colorKeys[0].time = 0.0f; colorKeys[0].color = Color.white;
		colorKeys[1].time = 1.0f; colorKeys[1].color = Color.white;
		colorGradient.SetKeys(colorKeys, alphaKeys);

		RandomlySpread();
	}

	void RandomlySpread(){
		for(int i = 0;i < count; ++i)
		{
			float minRate = Random.Range(Random.Range(
			                                  Random.Range(1.0f,1.8f),1.8f),1.8f);
			float maxRate = Random.Range(0.5f,Random.Range(
			                                    0.5f,Random.Range(0.5f,1.0f)));
			float radius = Random.Range(minradius * minRate, maxradius * maxRate);
			float angle = Random.Range(0.0f,360.0f);
			float time = Random.Range(0.0f,360.0f);
			circle[i] = new CirclePosition(radius,angle,time);
			particleArr[i].color = colorGradient.Evaluate(circle[i].angle / 360.0f);
		}
		particles.SetParticles(particleArr,particleArr.Length);
	}

	void Update ()
	{
	    for (int i = 0; i < count; i++)
	    {
		//粒子的转动半径随机变化
		circle[i].radius += Random.Range(-0.2f,0.2f);
		//控制粒子转动半径在一定范围内
		if(circle[i].radius < Random.Range(minradius-0.5f,minradius))
		   circle[i].radius += Random.Range(4.5f,5.5f);
		else if(circle[i].radius > Random.Range(maxradius,maxradius+0.5f))
		   circle[i].radius -= Random.Range(4.5f,5.5f);
		//控制粒子的转动速度在0.1f到0.2f之间随机
	        if (clockwise)
	            circle[i].angle -= Random.Range(0.1f,0.2f);
	        else
	            circle[i].angle += Random.Range(0.1f,0.2f);

	        circle[i].angle = (360.0f + circle[i].angle) % 360.0f;
	        float theta = circle[i].angle / 180 * Mathf.PI;

	        particleArr[i].position = new Vector3(circle[i].radius *
					         Mathf.Cos(theta), 0f, circle[i].radius * Mathf.Sin(theta));
	    }
	    particles.SetParticles(particleArr, particleArr.Length);
	}
}
