using Augmenta;
using Klak.Spout;
using UnityEngine;

namespace Augmenta
{
    public class AugmentaVideoOutputSpoutBinder : MonoBehaviour
    {
        [SerializeField] private SpoutSender spoutSender;
        [SerializeField] private AugmentaVideoOutput augmentaVideoOutput;

        private void OnEnable()
        {
            augmentaVideoOutput.videoOutputTextureUpdated += OnVideoOutputTextureUpdated;
            OnVideoOutputTextureUpdated();
        }

        private void OnDisable()
        {
            augmentaVideoOutput.videoOutputTextureUpdated -= OnVideoOutputTextureUpdated;
        }

        private void OnVideoOutputTextureUpdated()
        {
            spoutSender.sourceTexture = augmentaVideoOutput.videoOutputTexture;
        }
    }
}