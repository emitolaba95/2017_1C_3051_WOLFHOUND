﻿using Microsoft.DirectX;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TGC.Core.Geometry;
using TGC.Core.SkeletalAnimation;
using TGC.Core.Utils;

namespace TGC.Group.Model.Entities
{
    public abstract class Personaje
    {
        protected int maxHealth;
        protected int health;
        protected bool muerto;
        protected bool jumping;

        protected TgcSkeletalMesh esqueleto;
        protected Arma arma;

        protected float velocidadCaminar;
        protected float velocidadIzqDer;
        protected float velocidadRotacion;
        protected float tiempoSalto;
        protected float velocidadSalto;


        //CONSTRUCTORES
        public Personaje(string mediaDir, string skin, Vector3 initPosition)
        {
            muerto = false;
            health = maxHealth;
            loadSkeleton(mediaDir, skin);
            esqueleto.move(initPosition);
        }

        public Personaje(string mediaDir, string skin, Vector3 initPosition, Arma arma) :this(mediaDir, skin, initPosition)
        {
            setArma(arma);         
        }

        //METODOS

        //pongo virtual por si otro personaje requiera otras animaciones distintas, entonces cuando lo implementemos
        //solo tenemos que poner 'public override void loadPerson()'
        // skins: CS_Gign, CS_Arctic
        public virtual void loadSkeleton(string MediaDir, string skin)
        {
            //direccion del mesh
            var meshPath = MediaDir + "SkeletalAnimations\\BasicHuman\\" + skin + "-TgcSkeletalMesh.xml";
            //direccion para las texturas
            var mediaPath = MediaDir + "SkeletalAnimations\\BasicHuman\\";

            var skeletalLoader = new TgcSkeletalLoader();

            string[] animationList = {  "StandBy", "Walk", "Jump", "Run", "CrouchWalk" };

            var animationsPath = new string[animationList.Length];
            for (var i = 0; i < animationList.Length; i++)
            {
                //direccion de cada animacion
                animationsPath[i] = MediaDir + "SkeletalAnimations\\BasicHuman\\Animations\\" + animationList[i] + "-TgcSkeletalAnim.xml";
            }

            esqueleto = skeletalLoader.loadMeshAndAnimationsFromFile(meshPath,mediaPath, animationsPath );

            //Configurar animacion inicial    
            esqueleto.playAnimation("StandBy", true);
        }        

        public void recibiDanio(int danio)
        {
            if (danio >= health){
                health = 0;
                muerto = true;
            }
            else{
                health -= danio;
            }
        }

        public Vector3 Position{
            get { return esqueleto.Position; }
        }

        public void render(float elapsedTime)
        {
            esqueleto.Transform = Matrix.Translation(esqueleto.Position);
            esqueleto.animateAndRender(elapsedTime);
        }

        public void dispose()
        {
            esqueleto.dispose();
        }

        protected void setVelocidad(float caminar, float izqDer)
        {
            velocidadCaminar = caminar;
            velocidadIzqDer = izqDer;
        }
        
        //GETTERS Y SETTERS
        public TgcSkeletalMesh Esqueleto
        {
            get { return esqueleto; }
        }

        public void setArma(Arma arma)
        {
            if (this.arma != null)
            {
                this.arma.dispose();
            }
            arma.setPlayer(this);
        }
    }
}
