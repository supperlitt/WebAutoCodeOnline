Js文件：

qq_getsalt // 来源：qq处理登录uin使用
qq_getpwd  // 来源：qq处理登录salt,密码,验证码，合并成密码p
xxtea	   // 来源：cmf.cmpay.com，登录返回值解析
cmpay_rsa  // 来源：sina加密方式rsa,p.10086.com支付密码中使用，直接照搬未修改。
58_RSAUtils// 来源：58.com 处理用户登录，跟sina类似



类文件：
RSACryptoService.cs		// RSA加密，不包含 ----BEGIN   ----END
