using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System.IO;//allows to use file and csv

public class BossAnimationScript : MonoBehaviour {

    public Sprite openMouth;
    public Sprite closedMouth;
    int maxTimeBetweenFrames = 19;
    int minTimeBetweenFrames = 5;
    int ticksTilNextFrame = 40;
    Sprite[] frames;
    int currentFrame = 0;
    float lengthOfInterruption;
    public float timeElapsedInInterruption;
    BossMessage currentMessage;
    public float interruptionStartTime;
    public AudioClip audioTest;
    public AudioSource audioSource;
		
		public List<string> audioClipsName;
    public List<AudioClip> audioClips;
		public List<string> audioClipsText;
		
    public List<BossMessage> BossMessages;
    int currentMessageAudioClipIndex;
		
		public AudioClip boss001;
		public AudioClip boss002;
		public AudioClip boss003;
		public AudioClip boss004;
		public AudioClip boss005;
		public AudioClip boss006;
		public AudioClip boss007;
		public AudioClip boss008;
		public AudioClip boss009;
		public AudioClip boss010;
		
		public AudioClip boss011;
		public AudioClip boss012;
		public AudioClip boss013;
		public AudioClip boss014;
		public AudioClip boss015;
		public AudioClip boss016;
		public AudioClip boss017;
		public AudioClip boss018;
		public AudioClip boss019;
		public AudioClip boss020;

		public AudioClip boss021;
		public AudioClip boss022;
		public AudioClip boss023;
		public AudioClip boss024;
		public AudioClip boss025;
		public AudioClip boss026;
		public AudioClip boss027;
		public AudioClip boss028;
		public AudioClip boss029;
		public AudioClip boss030;
		
		public AudioClip boss031;
		public AudioClip boss032;
		public AudioClip boss033;
		public AudioClip boss034;
		public AudioClip boss035;
		public AudioClip boss036;
		public AudioClip boss037;
		public AudioClip boss038;
		public AudioClip boss039;
		public AudioClip boss040;
		
		public AudioClip boss041;
		public AudioClip boss042;
		public AudioClip boss043;
		public AudioClip boss044;
		public AudioClip boss045;
		public AudioClip boss046;
		public AudioClip boss047;
		public AudioClip boss048;
		public AudioClip boss049;
		public AudioClip boss050;
		
		public AudioClip boss051;
		
		public AudioClip boss054;
		public AudioClip boss055;
		public AudioClip boss056;
		public AudioClip boss057;
		public AudioClip boss058;
		public AudioClip boss059;
		public AudioClip boss060;
		
		public AudioClip boss061;
		public AudioClip boss062;
		public AudioClip boss063;
		public AudioClip boss064;
		public AudioClip boss065;
		
		public TextAsset bossAudioCSV;
		Csv csvObjectOfBossAudio;
		
		public GameObject subtitle;
		Text subtitleText;
		
		GameModeManagerScript gameModeManagerScript;
		
		public GameObject restartButton;
    
    // Use this for initialization
    void Start () {
        frames = new Sprite[2];
        frames[0] = openMouth;
        frames[1] = closedMouth;
        currentMessage = null;
				csvObjectOfBossAudio = new Csv(bossAudioCSV);
				
				for (int i=0; i<csvObjectOfBossAudio.contents.Count; i++)
				{
					audioClipsText.Add(csvObjectOfBossAudio.contents[i][1]);
				}
				

				
				setupBossAudioClips();
        gameObject.SetActive(false);

				//Debug.Log("subtitle text: " + subtitle.GetComponent<Text>().text);
				subtitleText = subtitle.GetComponent<Text>();
				
				gameModeManagerScript = GameObject.Find("Game Mode Manager").GetComponent<GameModeManagerScript>();
				
    }

    // Update is called once per frame
    void Update () {

        if (currentMessage == null)
        {
					if (gameModeManagerScript.areDead) 
					{
						subtitleText.text = "";
            
					} else {
						gameObject.SetActive(false);
					}
					
        } 
        ticksTilNextFrame--;
        if (ticksTilNextFrame < 0)
        {
            currentFrame++;
            if (currentFrame > frames.Length - 1)
            {
                currentFrame = 0;
            }
            ticksTilNextFrame = Random.Range(minTimeBetweenFrames, maxTimeBetweenFrames);
						
						if (currentMessage == null)
						{
						GetComponent<Image>().sprite = frames[1];	
						} else {
            GetComponent<Image>().sprite = frames[currentFrame];
					}
        }
        if (currentMessage != null)
        {
            timeElapsedInInterruption = Time.time - interruptionStartTime;

            if ( (timeElapsedInInterruption > lengthOfInterruption) )
            {
                currentMessage = null;
								
								if (GameObject.Find("Game Mode Manager").GetComponent<GameModeManagerScript>().areDead == false)
								{
                	gameObject.SetActive(false);
								} else {
									restartButton.SetActive(true);
								}
            }
            else
            {
                if (timeElapsedInInterruption > currentMessage.messageAudioClips[currentMessageAudioClipIndex].length + currentMessage.pausesBetweenClips[currentMessageAudioClipIndex])
                {
                    if (currentMessage.messageAudioClips.Count > currentMessageAudioClipIndex)
                    {
                        lengthOfInterruption -= currentMessage.messageAudioClips[currentMessageAudioClipIndex].length;

                        currentMessageAudioClipIndex++;

                        playAudioClip(currentMessage, currentMessageAudioClipIndex);
                        interruptionStartTime = Time.time;
                    }else
                    {
                        currentMessage = null;
                        gameObject.SetActive(false);
                    }
                }
            }
        }
	}

    public void playAudioClip(BossMessage message, int clipIndex)
    {
       audioSource.PlayOneShot(message.messageAudioClips[clipIndex]);
			 //Display Subtitles
			 subtitleText.text = message.script[clipIndex];
    }



    public void popUpMessage(BossMessage message)
    {
        Debug.Log("message = " + message);
        currentMessageAudioClipIndex = 0;
				/*
        if (message == null)
        {
            Debug.Log("popping message!");
            message = new BossMessage();
            message.audioClips = new AudioClip[1];
            message.audioClips[0] = audioTest;
            message.pausesBetweenClips = new float[1];
            message.pausesBetweenClips[0] = 0;
        }
				*/
        currentMessage = message;
        Debug.Log("audioSource = " + audioSource);
        playAudioClip(message, 0);
        lengthOfInterruption = 0;
        foreach (AudioClip clip in message.messageAudioClips)
        {
            lengthOfInterruption += clip.length;
        }
        foreach (float pause in message.pausesBetweenClips)
        {
            lengthOfInterruption += pause;
        }

        timeElapsedInInterruption = 0;
        interruptionStartTime = Time.time;
    }
		
		void setupBossAudioClips()
		{

      //audioClipsText = new List<string>();
      BossMessages = new List<BossMessage>();
			string file = Directory.GetCurrentDirectory() + "/Assets/Resources/BossAudioClips.csv";

			Csv csv = new Csv(bossAudioCSV);

      for (int i = 1; i < 10; i++)
      {
        audioClipsName.Add("boss00" + i + ".mp3"); //file name
     		//audioClipsText.Add("no text");	//text
			}
      for (int i = 10; i < 52; i++)
      {
         audioClipsName.Add("boss0" + i + ".mp3"); //file name
         //audioClipsText.Add("no text");  //text
      }

      audioClips.Add(boss001);	//0
			audioClips.Add(boss002);	//1
			audioClips.Add(boss003);	//2
			audioClips.Add(boss004);
			audioClips.Add(boss005);
			audioClips.Add(boss006);
			audioClips.Add(boss007);
			audioClips.Add(boss008);
			audioClips.Add(boss009);
			audioClips.Add(boss010);
			audioClips.Add(boss011);
			audioClips.Add(boss012);
			audioClips.Add(boss013);
			audioClips.Add(boss014);
			audioClips.Add(boss015);
			audioClips.Add(boss016);
			audioClips.Add(boss017);
			audioClips.Add(boss018);
			audioClips.Add(boss019);
			audioClips.Add(boss020);
			audioClips.Add(boss021);
			audioClips.Add(boss022);
			audioClips.Add(boss023);
			audioClips.Add(boss024);
			audioClips.Add(boss025);
			audioClips.Add(boss026);
			audioClips.Add(boss027);
			audioClips.Add(boss028);
			audioClips.Add(boss029);	
			audioClips.Add(boss030);
			audioClips.Add(boss031);
			audioClips.Add(boss032);
			audioClips.Add(boss033);
			audioClips.Add(boss034);
			audioClips.Add(boss035);
			audioClips.Add(boss036);
			audioClips.Add(boss037);
			audioClips.Add(boss038);
			audioClips.Add(boss039);	
			audioClips.Add(boss040);
			audioClips.Add(boss041);
			audioClips.Add(boss042);
			audioClips.Add(boss043);
			audioClips.Add(boss044);
			audioClips.Add(boss045);
			audioClips.Add(boss046);
			audioClips.Add(boss047);
			audioClips.Add(boss048);
			audioClips.Add(boss049);
			audioClips.Add(boss050);
			audioClips.Add(boss051);

			audioClips.Add(boss054);
			audioClips.Add(boss055);
			audioClips.Add(boss056);
			audioClips.Add(boss057);
			audioClips.Add(boss058);
			audioClips.Add(boss059);
			audioClips.Add(boss060);
			audioClips.Add(boss061);
			audioClips.Add(boss062);
			audioClips.Add(boss063);
			audioClips.Add(boss064);
			audioClips.Add(boss065);

      //BossMessages.Add(new BossMessage("intro", new List<int> (new int[] {0,1,2,3,4,5,6,7,8,9,10} )) );
      BossMessages.Add(new BossMessage("intro", new List<int> (new int[] {0,3,4,5,6,7,8,10} )) ); //removed thing about doors (9) and thing about crew at front of ship (10)

			//BossMessages.Add(new BossMessage("intro", new List<int> (new int[] {0,3,4,6,7,8} )) );//short
			//BossMessages.Add(new BossMessage("intro", new List<int> (new int[] {0} )) );//super short
			
			BossMessages.Add(new BossMessage("doGarbage", new List<int> (new int[] {1,2} )) );
			
			BossMessages.Add(new BossMessage("howI'mTalking", new List<int> (new int[] {1,2} )) );

      BossMessages.Add(new BossMessage("promotionToCaptain", new List<int> (new int[] {11, 12} )) );
			BossMessages.Add(new BossMessage("startAsCaptain", new List<int> (new int[] {13, 14, 15} )) );
			
			BossMessages.Add(new BossMessage("controlShip", new List<int> (new int[] {16} )) );
			BossMessages.Add(new BossMessage("navigationLesson", new List<int> (new int[] {18, 19} )) );
			BossMessages.Add(new BossMessage("toNavComputer", new List<int> (new int[] {20} )) );
			BossMessages.Add(new BossMessage("insideNavComputer", new List<int> (new int[] {21,22,23} )) );
			BossMessages.Add(new BossMessage("toProctaris", new List<int> (new int[] {24,25,26} )) );
			BossMessages.Add(new BossMessage("returnToBlackHole", new List<int> (new int[] {28,29} )) );
			BossMessages.Add(new BossMessage("shootPellet", new List<int> (new int[] {30} )) );
			BossMessages.Add(new BossMessage("shotPellet", new List<int> (new int[] {31} )) );
			BossMessages.Add(new BossMessage("biggerLoad", new List<int> (new int[] {32,33,34,35,36,37} )) );
			BossMessages.Add(new BossMessage("promotionToSectorManager", new List<int> (new int[] {38, 39} )) );
			BossMessages.Add(new BossMessage("promotionToDistrictManager", new List<int> (new int[] {40, 41} )) );

			
			BossMessages.Add(new BossMessage("blinkDrive", new List<int> (new int[] {42, 43} )) );
			BossMessages.Add(new BossMessage("blinkDrive2", new List<int> (new int[] {44} )) );
			
			BossMessages.Add(new BossMessage("PlanetExplode1", new List<int> (new int[] {45,46} )) );
			BossMessages.Add(new BossMessage("PlanetExplode2", new List<int> (new int[] {45,48} )) );
			BossMessages.Add(new BossMessage("PlanetExplode3", new List<int> (new int[] {47,48} )) );
			
			BossMessages.Add(new BossMessage("can'tFire", new List<int> (new int[] {49} )) );
			
			BossMessages.Add(new BossMessage("death1", new List<int> (new int[] {48,50} )) );
         //BossMessages.Add(new BossMessage("death2", new List<int> (new int[] {51} )) );
			
			BossMessages.Add(new BossMessage ("notFromNavComputer", new List<int> (new int[] {51, 52})) );
			
			BossMessages.Add(new BossMessage ("eyeOnStorage", new List<int> (new int[] {53, 54})) );
			
			BossMessages.Add(new BossMessage ("deathFromShip", new List<int> (new int[] {55, 56, 57, 58})) );
			BossMessages.Add(new BossMessage ("deathFromPlanet", new List<int> (new int[] {59, 60, 61, 62})) );

		}

    
		
    public void triggerBossMessage(string name)
    {   foreach (BossMessage message in BossMessages)
        {
            if (name == message.name)
            {   
							
                popUpMessage(message);
            }
        }
				GameObject.Find("PlayerPerson").GetComponent<PlayerPersonControls>().setArrowsToZero();
    }
}


public class BossMessage
{
    public string name;
    public List<AudioClip> messageAudioClips;
    public List<string> script; //text of audio clips
    public List<float> pausesBetweenClips;


		public BossMessage(string name, List<int> messageNumbers)
		{
			GameObject bossCanvas = GameObject.Find("Boss Canvas");
			
			BossAnimationScript bAnimationScript = bossCanvas.GetComponent<BossAnimationScript>();
      messageAudioClips = new List<AudioClip>();
      script = new List<string>();
      pausesBetweenClips = new List<float>();
      this.name = name;
			
			for (int i = 0; i<messageNumbers.Count; i++)
			{
				messageAudioClips.Add(bAnimationScript.audioClips[messageNumbers[i] ]);
				script.Add(bAnimationScript.audioClipsText[messageNumbers[i] ]);
				pausesBetweenClips.Add(0);
			}
			

		}
		
		public float messageLength()
		{
			float lengthOfInterruption = 0;
      foreach (AudioClip clip in this.messageAudioClips)
      {
          lengthOfInterruption += clip.length;
      }
      foreach (float pause in this.pausesBetweenClips)
      {
          lengthOfInterruption += pause;
      }
			return lengthOfInterruption;
		}
}




public class Csv
{
  public List<List <string> > contents = new List<List <string> >();
	string rowDeliniator = "\n";
	
	public Csv(string path, string filename){
		string filePath = generateFilePath(path, filename);
		makeCsv(filePath);
	}
	
	public Csv(string path){
		makeCsv(path);
		
	}
	
	public Csv(TextAsset text)
	{
		makeCsv(text);
	}
	
	void makeCsv(string path){
		//string wholeFile = File.ReadAllText(file);
		//string[] rows = wholeFile.Split("\n");
		List<string> rows = new List<string>();
		
		using(StreamReader reader = File.OpenText(path)){
			while (!reader.EndOfStream) {
				rows.Add(reader.ReadLine());
				
			}	
		}

		for (int i=0; i< rows.Count; i++){
			List<string> row = new List<string>();
			//row = rows[i].Split(',').ToList();
			row.AddRange(rows[i].Split(','));//AddRange fills a list with an array, I think
			
			contents.Add(row);
		}
		
		
	}
	
	void makeCsv(TextAsset text)
	{
		string csvString = text.ToString();
		
		List<string> rows = new List<string>();
		rows.AddRange(csvString.Split('\n') ); //AddRange fills a list with an array, I think
		
		for (int i=0; i< rows.Count; i++){
			List<string> row = new List<string>();
			row.AddRange(rows[i].Split(','));//AddRange fills a list with an array, I think
			
			row[1] = row[1].Replace("~", ",");
			
			contents.Add(row);
		}
		
	}
	
	string generateFilePath(string path, string fileName){
		string target = Directory.GetCurrentDirectory();
		target += "/" + path + "/";
		string file = target + fileName + ".csv";
		
		return file;
	}
	
	//void makeCSV(string filePath){
		
	//}
	
}


