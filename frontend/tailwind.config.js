/** @type {import('tailwindcss').Config} */
module.exports = {
  content: [
    "./src/**/*.{js,jsx,ts,tsx}",
  ],
  theme: {
    extend: {
      colors: {
        primary: {
          50: '#fef7f0',
          100: '#feeee1',
          200: '#fddcc2',
          300: '#fbc398',
          400: '#f7a16c',
          500: '#f2814a',
          600: '#e26b2f',
          700: '#bc5527',
          800: '#954426',
          900: '#793a23',
        },
        secondary: {
          50: '#f8fafc',
          100: '#f1f5f9',
          200: '#e2e8f0',
          300: '#cbd5e1',
          400: '#94a3b8',
          500: '#64748b',
          600: '#475569',
          700: '#334155',
          800: '#1e293b',
          900: '#0f172a',
        }
      },
      fontFamily: {
        'elegant': ['Georgia', 'Times New Roman', 'serif'],
      }
    },
  },
  plugins: [],
}
