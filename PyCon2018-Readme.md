# PyCon 2018
This steps reproduce the environment setup for PyCon 2018 Workshop on Image processing using CNTK/TensorFlow.
## CNTK Setup

1. Clone the repo
```powershell 
git clone https://github.com/pospanet/PyCon_2018.git
```
2. Go to cloned repo folder `PyCon_2018` and create new Python environment (assumed Anaconda/Miniconda installed already)

```powershell
cd PyCon_2018
conda create -y -n pycon2018 Python=3.5
activate pycon2018
pip install numpy scipy h5py opencv-python easydict pyyaml future pillow matplotlib
```
3. Install CNTK with appropriate version
- for CPU only (and Python 3.5)
```powershell
pip install https://cntk.ai/PythonWheel/CPU-Only/cntk-2.5.1-cp35-cp35m-win_amd64.whl
```
- or for GPU powered machine (and Python 3.5)
```powershell
--pip install https://cntk.ai/PythonWheel/GPU/cntk_gpu-2.5.1-cp35-cp35m-win_amd64.whl
```
4. Check the environment
```powershell
python --version
python -c "import cntk; print(cntk.__version__)"
```

## CNTK Run the lab
```powershell
python ImageDetection\PrepareData.py
cd ImageDetection
python Training.py
```
