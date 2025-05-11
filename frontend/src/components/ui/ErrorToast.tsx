import toast from "react-hot-toast";

const ErrorToast = (message: string) => {
    return toast(message, {
        duration: 2000,
        position: 'bottom-right',
        style: {
          background: 'red',
          color: '#fff',
        },
        iconTheme: {
          primary: '#fff',
          secondary: '#fff',
        }
    });
}

export default ErrorToast;