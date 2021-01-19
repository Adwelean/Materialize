namespace Assets.Scripts.NextGen.TextureSettings
{
    public class EdgeMapSettingsWrapper : EdgeSettings, ITextureSettings
    {
        public EdgeMapSettingsWrapper()
        {
            SetWeightEqDefault();
        }

        private void SetWeightEqDefault()
        {
            Blur0Weight = 1.0f;
            Blur1Weight = 0.5f;
            Blur2Weight = 0.3f;
            Blur3Weight = 0.5f;
            Blur4Weight = 0.7f;
            Blur5Weight = 0.7f;
            Blur6Weight = 0.3f;
        }

        private void SetWeightEqDisplace()
        {
            Blur0Weight = 0.1f;
            Blur1Weight = 0.15f;
            Blur2Weight = 0.25f;
            Blur3Weight = 0.45f;
            Blur4Weight = 0.75f;
            Blur5Weight = 0.95f;
            Blur6Weight = 1.0f;

            Blur0Contrast = 3.0f;
            Blur0ContrastText = "3";

            FinalContrast = 5.0f;
            FinalContrastText = "5";

            FinalBias = -0.2f;
            FinalBiasText = "-0.2";
        }

        private void SetWeightEqSoft()
        {
            Blur0Weight = 0.15f;
            Blur1Weight = 0.4f;
            Blur2Weight = 0.7f;
            Blur3Weight = 0.9f;
            Blur4Weight = 1.0f;
            Blur5Weight = 0.9f;
            Blur6Weight = 0.7f;

            FinalContrast = 4.0f;
            FinalContrastText = "4";
        }

        private void SetWeightEqTight()
        {
            Blur0Weight = 1.0f;
            Blur1Weight = 0.45f;
            Blur2Weight = 0.25f;
            Blur3Weight = 0.18f;
            Blur4Weight = 0.15f;
            Blur5Weight = 0.13f;
            Blur6Weight = 0.1f;

            Pinch = 1.5f;
            PinchText = "1.5";
        }
    }
}
