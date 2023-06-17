using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.XR.ARFoundation;

public class TrackedImagePlacer : MonoBehaviour
{
    public ARTrackedImageManager trackedImageManager;
    public List<GameObject> arPrefabs;
    public TextMeshProUGUI text;
    private List<GameObject> _instantiatedPrefabs = new List<GameObject>();
    private Dictionary<string, GameObject> _arPrefabsDict = new Dictionary<string, GameObject>(); // Diccionario que relaciona un string con un prefab, de esta manera puedo acceder al prefab a traves de su nombre
    private Dictionary<string, GameObject> _instantiatedPrefabsDict = new Dictionary<string, GameObject>(); // diccionario que cumple lo mismo que el anterior pero para objetos que ya hayan sido creados en la escena

    private void OnEnable()
    {
        trackedImageManager.trackedImagesChanged += OnTrackedImageChangeV2; //Aqui se agrega el metodo que se quiera usar (el basico o el mas avanzado)
    }

    private void OnDisable()
    {
        trackedImageManager.trackedImagesChanged -= OnTrackedImageChangeV2;
    }

    private void Start()
    {
        foreach (var prefab in arPrefabs) //Para llenar el diccionario con el nombre y el objeto
        {
            _arPrefabsDict.Add(prefab.name, prefab);
        }
    }

    private void OnTrackedImageChange(ARTrackedImagesChangedEventArgs args) //Versión básica (con un ciclo dentro de otro)
    {
        foreach (var trackedImage in args.added)
        {
            var imageName = trackedImage.referenceImage.name;
            foreach (var prefab in arPrefabs)
            {
                if (prefab.name.Equals(imageName))
                {
                    var instantiatedPrefab = Instantiate(prefab, trackedImage.transform);
                    _instantiatedPrefabs.Add(instantiatedPrefab);
                }
            }
        }
        
        foreach (var trackedImage in args.removed)
        {
            var imageName = trackedImage.referenceImage.name;
            foreach (var prefab in _instantiatedPrefabs)
            {
                if (prefab.name.Equals(imageName))
                {
                    _instantiatedPrefabs.Remove(prefab);
                    Destroy(prefab);
                }
            }
        }
    }

    private void OnTrackedImageChangeV2(ARTrackedImagesChangedEventArgs args)
    {
        foreach (var trackedImage in args.added)
        {
            var imageName = trackedImage.referenceImage.name;
            if (_arPrefabsDict.ContainsKey(imageName)) // pregunto si el diccionario de prefabs tiene el nombre de la imagen
            {
                
                var prefab = _arPrefabsDict[imageName]; // obtengo el prefab a traves del nombre de la imagen en el diccionario
                var instantiatedPrefab = Instantiate(prefab, trackedImage.transform);
                _instantiatedPrefabsDict.Add(imageName, instantiatedPrefab); // añado el objeto creado al diccionario de objetos creados
            }
        }
        
        foreach (var trackedImage in args.removed)
        {
            var imageName = trackedImage.referenceImage.name;
            if (_instantiatedPrefabsDict.ContainsKey(imageName))
            {
                var objectToDestroy = _instantiatedPrefabsDict[imageName]; // obtengo el objeto que voy a destruir
                _instantiatedPrefabsDict.Remove(imageName); // saco del diccionario el objeto que voy a destruir
                Destroy(objectToDestroy);
            }
        }
    }
}
