import type {Config} from 'tailwindcss';
import colors from "tailwindcss/colors";

export default <Partial<Config>>{
    darkMode: 'class',
    content: [
        'nuxt.config.ts'
    ],
    theme: {
        extend: {
            fontFamily: {
                sans: ['Manrope', 'sans-serif'],
                serif: ['Manrope', 'serif']
            },
            colors: {
                primary: colors.cyan
            }
        }
    },
    plugins: [
        require('@tailwindcss/forms')
    ]
}