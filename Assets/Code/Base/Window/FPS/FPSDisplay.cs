using TMPro;
using UnityEngine;

[RequireComponent(typeof(FPSCounter))]
public class FPSDisplay : MonoBehaviour {

	[System.Serializable]
	private struct FPSColor {
		public Color color;
		public int minimumFPS;
	}

	public TextMeshProUGUI averageFPSLabel, lowestFPSLabel;

	[SerializeField]
	private FPSColor[] coloring;

	FPSCounter fpsCounter;


	void Awake () {
		fpsCounter = GetComponent<FPSCounter>();
	}

	void Update () {
		Display(averageFPSLabel, fpsCounter.AverageFPS);
		Display(lowestFPSLabel, fpsCounter.LowestFPS);
	}

	void Display (TextMeshProUGUI label, int fps) {
		label.text = Mathf.Clamp(fps, 0, 99).Opt00Str();
		for (int i = 0; i < coloring.Length; i++) {
			if (fps >= coloring[i].minimumFPS) {
				label.color = coloring[i].color;
				break;
			}
		}
	}
}