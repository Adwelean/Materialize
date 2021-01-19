namespace Assets.Scripts.NextGen.TextureSettings
{
    public class HeightMapSettingsWrapper : HeightFromDiffuseSettings, ITextureSettings
    {
        public HeightMapSettingsWrapper()
        {
            SetWeightEqDetail();
            SetContrastEqDefault();
        }

        public void SetWeightEqDefault()
        {
            this.Blur0Weight = 0.15f;
            this.Blur1Weight = 0.19f;
            this.Blur2Weight = 0.3f;
            this.Blur3Weight = 0.5f;
            this.Blur4Weight = 0.7f;
            this.Blur5Weight = 0.9f;
            this.Blur6Weight = 1.0f;
        }

        public void SetWeightEqDetail()
        {
            this.Blur0Weight = 0.7f;
            this.Blur1Weight = 0.4f;
            this.Blur2Weight = 0.3f;
            this.Blur3Weight = 0.5f;
            this.Blur4Weight = 0.8f;
            this.Blur5Weight = 0.9f;
            this.Blur6Weight = 0.7f;
        }

        public void SetWeightEqDisplace()
        {
            this.Blur0Weight = 0.02f;
            this.Blur1Weight = 0.03f;
            this.Blur2Weight = 0.1f;
            this.Blur3Weight = 0.35f;
            this.Blur4Weight = 0.7f;
            this.Blur5Weight = 0.9f;
            this.Blur6Weight = 1.0f;
        }

        public void SetContrastEqDefault()
        {
            this.Blur0Contrast = 1.0f;
            this.Blur1Contrast = 1.0f;
            this.Blur2Contrast = 1.0f;
            this.Blur3Contrast = 1.0f;
            this.Blur4Contrast = 1.0f;
            this.Blur5Contrast = 1.0f;
            this.Blur6Contrast = 1.0f;
        }

        public void SetContrastEqCrackedMud()
        {
            this.Blur0Contrast = 1.0f;
            this.Blur1Contrast = 1.0f;
            this.Blur2Contrast = 1.0f;
            this.Blur3Contrast = 1.0f;
            this.Blur4Contrast = -0.2f;
            this.Blur5Contrast = -2.0f;
            this.Blur6Contrast = -4.0f;
        }

        public void SetContrastEqFunky()
        {
            this.Blur0Contrast = -3.0f;
            this.Blur1Contrast = -1.2f;
            this.Blur2Contrast = 0.30f;
            this.Blur3Contrast = 1.3f;
            this.Blur4Contrast = 2.0f;
            this.Blur5Contrast = 2.5f;
            this.Blur6Contrast = 2.0f;
        }
    }
}
