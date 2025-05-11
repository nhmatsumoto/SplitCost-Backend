import toast from "react-hot-toast";

const SuccessToast = (message: string) => {
    return toast(message, {
        duration: 2000,
        position: 'bottom-right',
        style: {
          background: 'green',
          color: '#fff',
        },
        iconTheme: {
          primary: '#fff',
          secondary: '#fff',
        }
    });
}

export default SuccessToast;