namespace Assets.Scripts.NextGen.TextureSettings
{
    public class NormalMapSettingsWrapper : NormalFromHeightSettings, ITextureSettings
    {
        public NormalMapSettingsWrapper()
        {
            SetWeightEqMids();
        }

        private void SetWeightEqDefault()
        {
            this.Blur0Weight = 0.3f;
            this.Blur1Weight = 0.35f;
            this.Blur2Weight = 0.5f;
            this.Blur3Weight = 0.8f;
            this.Blur4Weight = 1.0f;
            this.Blur5Weight = 0.95f;
            this.Blur6Weight = 0.8f;
        }

        private void SetWeightEqSmooth()
        {
            this.Blur0Weight = 0.1f;
            this.Blur1Weight = 0.15f;
            this.Blur2Weight = 0.25f;
            this.Blur3Weight = 0.6f;
            this.Blur4Weight = 0.9f;
            this.Blur5Weight = 1.0f;
            this.Blur6Weight = 1.0f;
        }

        private void SetWeightEqCrisp()
        {
            this.Blur0Weight = 1.0f;
            this.Blur1Weight = 0.9f;
            this.Blur2Weight = 0.6f;
            this.Blur3Weight = 0.4f;
            this.Blur4Weight = 0.25f;
            this.Blur5Weight = 0.15f;
            this.Blur6Weight = 0.1f;
        }

        private void SetWeightEqMids()
        {
            this.Blur0Weight = 0.15f;
            this.Blur1Weight = 0.5f;
            this.Blur2Weight = 0.85f;
            this.Blur3Weight = 1.0f;
            this.Blur4Weight = 0.85f;
            this.Blur5Weight = 0.5f;
            this.Blur6Weight = 0.15f;
        }
    }
}
