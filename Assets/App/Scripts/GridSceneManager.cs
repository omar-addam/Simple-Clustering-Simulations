using Clustering.DBScan;
using Clustering.KMeans;
using Clustering.KMedoids;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class GridSceneManager : MonoBehaviour
{

	#region Variables

	/// <summary>
	/// References the grid in the scene.
	/// </summary>
	public Grid GridManager;

	/// <summary>
	/// References the title UI text element.
	/// </summary>
	public Text TitleUIText;

	/// <summary>
	/// References the iterations title UI text element.
	/// </summary>
	public Text IterationsUIText;

	/// <summary>
	/// References thje iterations slider UI element.
	/// </summary>
	public Slider IterationsSlider;

	/// <summary>
	/// References the algorithm scriptable object.
	/// </summary>
	public AlgorithmManagerScriptableObject AlgorithmManager;

	/// <summary>
	/// The color associated with each cluster.
	/// </summary>
	private AbstractGridVisualizer Visualizer;

	#endregion

	#region Initialization

	/// <summary>
	/// Executes once on start.
	/// </summary>
	private void Awake()
	{
		// Initialize visualizer
		if (AlgorithmManager.CurrentAlgorithm is KMeansAlgorithm)
			Visualizer = new KMeansGridVisualizer(AlgorithmManager, GridManager);
		else if (AlgorithmManager.CurrentAlgorithm is KMedoidsAlgorithm)
			Visualizer = new KMedoidsGridVisualizer(AlgorithmManager, GridManager);
		else if (AlgorithmManager.CurrentAlgorithm is DBScanAlgorithm)
			Visualizer = new DBScanGridVisualizer(AlgorithmManager, GridManager);

		// Display title
		TitleUIText.text = AlgorithmManager.CurrentAlgorithm.ToString();

		// Display iterations
		InitializeIterationsSlider();
	}

	#endregion

	#region Methods

	/// <summary>
	/// Goes back to the introduction scene.
	/// </summary>
	public void LoadIntroductionScene()
	{
		SceneManager.LoadScene("IntroductionScene", LoadSceneMode.Single);
	}

	/// <summary>
	/// Initializes the slider.
	/// </summary>
	private void InitializeIterationsSlider()
	{
		int numberOfIterations = Visualizer.GetIterationsCount();

		// Display on slider
		IterationsUIText.text = string.Format("Iteration: {0} / {1}", 0, numberOfIterations - 1);
		IterationsSlider.maxValue = numberOfIterations - 1;
		IterationsSlider.onValueChanged.AddListener((float value) =>
		{
			IterationsUIText.text = string.Format("Iteration: {0} / {1}", value, numberOfIterations - 1);

			// Display iteration entities
			DisplayIteration((int)value);
		});
	}

	/// <summary>
	/// Displays the entities of an iteration.
	/// </summary>
	private void DisplayIteration(int iterationNumber)
	{
		Visualizer.DisplayIteration(iterationNumber);
	}

	#endregion

}
