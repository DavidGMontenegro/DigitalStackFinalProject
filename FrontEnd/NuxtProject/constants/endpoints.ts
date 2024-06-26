const ip = "localhost:5076";

export const loginEndPoint = "http://" + ip + "/api/Users/login";
export const signupEndPoint = "http://" + ip + "/api/Users/register";
export const getUserDataEndPoint = "http://" + ip + "/api/Users/Get-user";
export const changePasswordEndPoint = "http://" + ip + "/api/Users/change-user-password";
export const changeProfilePicEndPoint = "http://" + ip + "/api/Users/change-user-picture";
export const modifyUserDataEndPoint = "http://" + ip + "/api/Users/modify-user-data";

export const sendEmailEndPoint = "http://" + ip + "/api/Mail/send-email";

export const getDataEndPoint = "http://" + ip + "/api/Data/get-all-data";
export const priceByMakeEndPoint = "http://" + ip + "/api/Data/price-distribution-by-make";
export const transmissionTypeEndPoint = "http://" + ip + "/api/Data/transmission-type-distribution";
export const priceByYearEndPoint = "http://" + ip + "/api/Data/price-trend-by-year";
export const fuelTypeEndPoint = "http://" + ip + "/api/Data/fuel-type-distribution";
export const yearDistributionEndPoint = "http://" + ip + "/api/Data/year-distribution";
export const getBrandsEndPoints = "http://" + ip + "/api/Data/get-car-brands";
export const getModelsByMakeEndPoints = "http://" + ip + "/api/Data/get-cars-by-brand";
export const getPoweByMakeAndModelEndPoints = "http://" + ip + "/api/Data/powers";
export const getColorsByMakeAndModelEndPoints = "http://" + ip + "/api/Data/colors";