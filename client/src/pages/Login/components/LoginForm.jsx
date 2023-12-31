import {Link} from "react-router-dom";
import * as yup from 'yup'
import {useForm} from "react-hook-form";
import {yupResolver} from "@hookform/resolvers/yup";
import {GoogleLogin, GoogleOAuthProvider} from "@react-oauth/google";
import Axios from "axios";

function LoginForm() {
    const signInSchema = yup.object().shape({
        email: yup.string().required("Email field is required").email("Invalid format"),
        password: yup.string().required("Password is required")
    })

    const {register, handleSubmit, formState: {errors}} = useForm({
        resolver: yupResolver(signInSchema)
    })

    const handleLogin = (data) => {
        console.log(data)
    }

    const googleResponseMessage = (response) => {
        Axios.post("https://localhost:5001/auth/google", {accessToken: response.credential})
            .then(res => console.log(res))
    };
    const googleErrorMessage = (error) => {
        console.log(error);
    };

    return (
        <section className="bg-gray-50 dark:bg-gray-900">
            <GoogleOAuthProvider clientId="1005035136595-0rsm8fkuom187f4r0c93uon9jh7tre8g.apps.googleusercontent.com">
                <div className="flex flex-col items-center justify-center px-6 py-8 mx-auto md:h-screen lg:py-0">
                    <div
                        className="w-full bg-white rounded-lg shadow dark:border md:mt-0 sm:max-w-md xl:p-0 dark:bg-gray-800 dark:border-gray-700">
                        <div className="p-6 space-y-4 md:space-y-6 sm:p-8">
                            <h1 className="text-xl font-bold leading-tight tracking-tight text-gray-900 md:text-2xl dark:text-white">
                                Sign in to your account
                            </h1>
                            <form className="space-y-4 md:space-y-6" onSubmit={handleSubmit(handleLogin)} >
                                <div>
                                    <label htmlFor="email"
                                           className="block mb-2 text-sm font-medium text-gray-900 dark:text-white">Your
                                        email</label>
                                    <input type="email" name="email" id="email"
                                           {...register("email")}
                                           className="bg-gray-50 border border-gray-300 text-gray-900 sm:text-sm rounded-lg focus:ring-primary-600 focus:border-primary-600 block w-full p-2.5 dark:bg-gray-700 dark:border-gray-600 dark:placeholder-gray-400 dark:text-white dark:focus:ring-blue-500 dark:focus:border-blue-500"
                                           placeholder="name@company.com" required="" />
                                    <p>{errors.email?.message}</p>
                                </div>
                                <div>
                                    <label htmlFor="password"
                                           className="block mb-2 text-sm font-medium text-gray-900 dark:text-white">Password</label>
                                    <input type="password" name="password" id="password" placeholder="••••••••"
                                           {...register("password")}
                                           className="bg-gray-50 border border-gray-300 text-gray-900 sm:text-sm rounded-lg focus:ring-primary-600 focus:border-primary-600 block w-full p-2.5 dark:bg-gray-700 dark:border-gray-600 dark:placeholder-gray-400 dark:text-white dark:focus:ring-blue-500 dark:focus:border-blue-500"
                                           required="" />
                                    <p>{errors.password?.message}</p>
                                </div>
                                <div className="flex items-center justify-between">
                                    <div className="flex items-start">
                                        <div className="flex items-center h-5">
                                            <input id="remember" aria-describedby="remember" type="checkbox"
                                                   className="w-4 h-4 border border-gray-300 rounded bg-gray-50 focus:ring-3 focus:ring-primary-300 dark:bg-gray-700 dark:border-gray-600 dark:focus:ring-primary-600 dark:ring-offset-gray-800"
                                                   required="" />
                                        </div>
                                        <div className="ml-3 text-sm">
                                            <label htmlFor="remember" className="text-gray-500 dark:text-gray-300">Remember
                                                me</label>
                                        </div>
                                    </div>
                                    <span
                                        className="text-sm font-medium text-primary-600 hover:underline dark:text-primary-500">
                                    Forgot password?
                                </span>
                                </div>
                                <button type="submit"
                                        className="w-full bg-primary-600 hover:bg-primary-700 focus:ring-4 focus:outline-none focus:ring-primary-300 font-medium rounded-lg text-sm px-5 py-2.5 text-center dark:bg-primary-600 dark:hover:bg-primary-700 dark:focus:ring-primary-800">
                                    Sign in
                                </button>
                                <p className="text-sm font-light text-gray-500 dark:text-gray-400">
                                    Don’t have an account yet?
                                    <Link to="/sign-up"
                                          className="font-medium ml-2 text-primary-600 hover:underline dark:text-primary-500">
                                        Sign up
                                    </Link>
                                </p>
                            </form>
                            <div>
                                <GoogleLogin onSuccess={googleResponseMessage} onError={googleErrorMessage}  />
                            </div>
                        </div>
                    </div>
                </div>
            </GoogleOAuthProvider>
        </section>
    );
}

export default LoginForm;