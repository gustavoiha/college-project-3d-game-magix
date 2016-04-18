﻿using UnityEngine;
using System.Collections;

public class SkillsController : MonoBehaviour {

	//Botões
    public KeyCode magicOne;
    public KeyCode magicTwo;
    public KeyCode magicThree;
    public KeyCode magicFour;
    public KeyCode magicFive;

    //Target acquired at ENEMY BEHAVIOUR script
    public GameObject target;

	///
	/// Skills' prefabs
	///

	//Prefab da flecha de luz
	public GameObject lightArrow; 
	//Prefab da bola de luz
    public GameObject lightBall;
	//Prefab da LightCross
    public GameObject lightCross;
    //Prefab EssenceStealer
    public GameObject essenceStealer;

	// Skills' id's. Pass them as arguments int UseSkill() method
	public const string LIGHT_ARROW = "lightArrow";
	public const string LIGHT_BALL  = "lightBall";
	public const string LIGHT_CROSS = "lightCross";

	//Força aplicada na flecha
    public float lightArrowForce = 100.0f; 

	// Força aplicada na esfera
    public float lightBallForce = 50.0f; 

	// Força aplicada no raio
    public float lightCrossForce = 10.0f;

	//Direção em q o mouse aponta
    Vector3 direction; 

	//localização temporária para atirar magia(colocar a mão/cajado aqui futuramente)
    Vector3 location; 

	// Use this for initialization
	void Start () {
	    
	}

    // Update is called once per frame
    void Update() {

        //If enemy has no health, the target will become null
        if(target != null)
            if (target.GetComponent<HealthController>().health <= 0)
                target = null;

        location = transform.position + Vector3.forward * 3; //Coloca a posição um pouco a frente

        if (target == null) //If there is no target at all, calculations will be based on the mouse position
        {
            var worldPosition = Camera.main.ScreenToWorldPoint(new Vector3(Input.mousePosition.x, Input.mousePosition.y, transform.position.z)); //Calcula local do mouse em relação a tela
            direction = worldPosition - transform.position; //Calcula direção baseada entre o personagem e o mouse
            direction.Normalize();//Normaliza o vetor
        } else //If there is a target, calculations will be based on the target's position
        {
            direction = target.transform.position - location;
            direction.Normalize();
        }
        if (Input.GetKey(magicOne)) //Ao apertar o botão 1, usa a primeira magia
            UseLightArrow();
        if (Input.GetKeyDown(magicTwo))//Ao apertar o botão 2, usa a segunda
            UseLightBall();
        if (Input.GetKeyDown(magicThree))//Ao apertar o botão 3, usa a terceira
            UseLightCross();
        if (Input.GetKeyDown(magicFour))//Ao apertar o botão 4, acionamosu desativamo santuário
            LightSanctuaryBehaviour.toogleSanctuary = !LightSanctuaryBehaviour.toogleSanctuary;
        if (Input.GetKeyDown(magicFive))
        {
            if (target == null)
                print("No target!");
            else
                UseEssenceStealer();
        }

        //Seria bom colocar um static pra quando coletar os outros amuletos destrancar as magias
    }

	/// <summary>
	/// Call his method to use a desired skill
	/// </summary>
	/// <param name="skill">Skill.</param>
	public void UseSkill (string skillID){

		// Check if null or empty to continue
		if (string.IsNullOrEmpty(skillID))
			return;
		
		switch (skillID) {

		case LIGHT_ARROW:
			UseLightArrow ();
			break;
		case LIGHT_BALL:
			UseLightBall ();
			break;
		case LIGHT_CROSS:
			UseLightCross ();
			break;
		}

	}

    private void UseLightArrow() {
        //Instantiate novos projéteis (Euler foi necessário para endireitar o prefab)
        GameObject projectile = Instantiate(lightArrow, location, Quaternion.LookRotation(direction) * Quaternion.Euler(90, 0, 180)) as GameObject;
        projectile.GetComponent<Rigidbody>().useGravity = false; //Evita a gravidade
        projectile.GetComponent<Rigidbody>().AddForce(direction * lightArrowForce); //Coloca uma força para arremessar a magia
    }

	private void UseLightBall() {
        //Mesma coisa acima, sem o ajuste, já q se trata de uma esfera
        GameObject projectile = Instantiate(lightBall, location, Quaternion.LookRotation(direction)) as GameObject;
        projectile.GetComponent<Rigidbody>().useGravity = false;
        projectile.GetComponent<Rigidbody>().AddForce(direction * lightBallForce);
    }

	private void UseLightCross() {
        //Cura aqui
       Instantiate(lightCross, transform.position, Quaternion.LookRotation(direction)); //Cria uma LightCross no local do player
    }

    private void UseEssenceStealer()
    {
            essenceStealer.transform.position = target.transform.position;
            if (!essenceStealer.GetComponent<ParticleSystem>().isPlaying)
                essenceStealer.GetComponent<ParticleSystem>().Play();
            if(Input.GetKeyUp(magicFive))
                essenceStealer.GetComponent<ParticleSystem>().Stop();
    }
}
