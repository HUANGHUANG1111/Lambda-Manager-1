#ifndef __CUDA_H
#define __CUDA_H

#include "my_cuda.cuh"



class Mycuda : public CUDA_class
{
public:
	Mycuda();

public:
	void grid_creat_now(cv::Mat* dst, int nH, int nW, float delta_x, float delta_y, int i);
	void circ_creat(cv::Mat src, cv::Mat& dst1, cv::Mat& dst2, float Max_frequency, float Filter, int i);
	void filter_creat_now(cv::Mat src1, cv::Mat src2, cv::Mat& dst1, cv::Mat& dst2, bool flag, int i);
	void ALL_calculate(cv::Mat Phi, cv::Mat real_filter, cv::Mat imag_filter, cv::Mat* Ipc, int nH_extend, int nW_extend, int i);
	//void gaussianBlur_gpu(cv::Mat& src, cv::Mat& dst, int Gas_Radius, int Gas_var);
	void Set_lock(int i);
	void Set_unlock(int i);
	bool Get_LockState(int i);

public:
	//num�����������ã��������������
	int num = 0;

	//�ɵ�����
	int nW_old = 0;
	int nH_old = 0;
	//�ɵ��˲�����
	float Max_frequency_old = 0.0f;
	float Filter_old = 0.0f;
	//����һ��ȫ��rho(Phase2PC)
	cv::Mat rho_old;
	//����һ��ȫ��real(Phase2PC)
	cv::Mat real_old;
	//����һ��ȫ��imag(Phase2PC)
	cv::Mat imag_old;

	std::mutex mtx;

	enum PC_state
	{
		no_change,
		size_change,
		filter_change
	};

private:
	bool Lock_Flag[4];
	
};

#endif // !__CUDA_H
