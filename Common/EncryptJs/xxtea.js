var window = window || {};

!function (t) {
    "use strict";
    function e(t, e) {
        var n = t.length, r = n << 2;
        if (e) {
            var i = t[n - 1];
            if (r -= 4, r - 3 > i || i > r)
                return null;
            r = i;
        }
        for (var o = 0; n > o; o++)
            t[o] = String.fromCharCode(255 & t[o], t[o] >>> 8 & 255, t[o] >>> 16 & 255, t[o] >>> 24 & 255);
        var s = t.join("");
        return e ? s.substring(0, r) : s;
    }
    function n(t, e) {
        var n = t.length, r = n >> 2;
        0 !== (3 & n) && ++r;
        var i;
        e ? (i = new Array(r + 1), i[r] = n) : i = new Array(r);
        for (var o = 0; n > o; ++o)
            i[o >> 2] |= t.charCodeAt(o) << ((3 & o) << 3);
        return i;
    }
    function r(t) {
        return 4294967295 & t;
    }
    function i(t, e, n, r, i, o) {
        return (n >>> 5 ^ e << 2) + (e >>> 3 ^ n << 4) ^ (t ^ e) + (o[3 & r ^ i] ^ n);
    }
    function o(t) {
        return t.length < 4 && (t.length = 4), t;
    }
    function s(t, e) {
        var n, o, s, a, u, c, d = t.length, f = d - 1;
        for (o = t[f], s = 0, c = 0 | Math.floor(6 + 52 / d) ; c > 0; --c) {
            for (s = r(s + y), a = s >>> 2 & 3, u = 0; f > u; ++u)
                n = t[u + 1], o = t[u] = r(t[u] + i(s, n, o, u, a, e));
            n = t[0], o = t[f] = r(t[f] + i(s, n, o, f, a, e));
        }
        return t;
    }
    function a(t, e) {
        var n, o, s, a, u, c, d = t.length, f = d - 1;
        for (n = t[0], c = Math.floor(6 + 52 / d), s = r(c * y) ; 0 !== s; s = r(s - y)) {
            for (a = s >>> 2 & 3, u = f; u > 0; --u)
                o = t[u - 1], n = t[u] = r(t[u] - i(s, n, o, u, a, e));
            o = t[f], n = t[0] = r(t[0] - i(s, n, o, 0, a, e));
        }
        return t;
    }
    function u(t) {
        if (/^[\x00-\x7f]*$/.test(t))
            return t;
        for (var e = [], n = t.length, r = 0, i = 0; n > r; ++r, ++i) {
            var o = t.charCodeAt(r);
            if (128 > o)
                e[i] = t.charAt(r);
            else if (2048 > o)
                e[i] = String.fromCharCode(192 | o >> 6, 128 | 63 & o);
            else {
                if (!(55296 > o || o > 57343)) {
                    if (n > r + 1) {
                        var s = t.charCodeAt(r + 1);
                        if (56320 > o && s >= 56320 && 57343 >= s) {
                            var a = ((1023 & o) << 10 | 1023 & s) + 65536;
                            e[i] = String.fromCharCode(240 | a >> 18 & 63, 128 | a >> 12 & 63, 128 | a >> 6 & 63, 128 | 63 & a), ++r;
                            continue;
                        }
                    }
                    throw new Error("Malformed string");
                }
                e[i] = String.fromCharCode(224 | o >> 12, 128 | o >> 6 & 63, 128 | 63 & o);
            }
        }
        return e.join("");
    }
    function c(t, e) {
        for (var n = new Array(e), r = 0, i = 0, o = t.length; e > r && o > i; r++) {
            var s = t.charCodeAt(i++);
            switch (s >> 4) {
                case 0:
                case 1:
                case 2:
                case 3:
                case 4:
                case 5:
                case 6:
                case 7:
                    n[r] = s;
                    break;
                case 12:
                case 13:
                    if (!(o > i))
                        throw new Error("Unfinished UTF-8 octet sequence");
                    n[r] = (31 & s) << 6 | 63 & t.charCodeAt(i++);
                    break;
                case 14:
                    if (!(o > i + 1))
                        throw new Error("Unfinished UTF-8 octet sequence");
                    n[r] = (15 & s) << 12 | (63 & t.charCodeAt(i++)) << 6 | 63 & t.charCodeAt(i++);
                    break;
                case 15:
                    if (!(o > i + 2))
                        throw new Error("Unfinished UTF-8 octet sequence");
                    var a = ((7 & s) << 18 | (63 & t.charCodeAt(i++)) << 12 | (63 & t.charCodeAt(i++)) << 6 | 63 & t.charCodeAt(i++)) - 65536;
                    if (!(a >= 0 && 1048575 >= a))
                        throw new Error("Character outside valid Unicode range: 0x" + a.toString(16));
                    n[r++] = a >> 10 & 1023 | 55296, n[r] = 1023 & a | 56320;
                    break;
                default:
                    throw new Error("Bad UTF-8 encoding 0x" + s.toString(16));
            }
        }
        return e > r && (n.length = r), String.fromCharCode.apply(String, n);
    }
    function d(t, e) {
        for (var n = [], r = new Array(65535), i = 0, o = 0, s = t.length; e > i && s > o; i++) {
            var a = t.charCodeAt(o++);
            switch (a >> 4) {
                case 0:
                case 1:
                case 2:
                case 3:
                case 4:
                case 5:
                case 6:
                case 7:
                    r[i] = a;
                    break;
                case 12:
                case 13:
                    if (!(s > o))
                        throw new Error("Unfinished UTF-8 octet sequence");
                    r[i] = (31 & a) << 6 | 63 & t.charCodeAt(o++);
                    break;
                case 14:
                    if (!(s > o + 1))
                        throw new Error("Unfinished UTF-8 octet sequence");
                    r[i] = (15 & a) << 12 | (63 & t.charCodeAt(o++)) << 6 | 63 & t.charCodeAt(o++);
                    break;
                case 15:
                    if (!(s > o + 2))
                        throw new Error("Unfinished UTF-8 octet sequence");
                    var u = ((7 & a) << 18 | (63 & t.charCodeAt(o++)) << 12 | (63 & t.charCodeAt(o++)) << 6 | 63 & t.charCodeAt(o++)) - 65536;
                    if (!(u >= 0 && 1048575 >= u))
                        throw new Error("Character outside valid Unicode range: 0x" + u.toString(16));
                    r[i++] = u >> 10 & 1023 | 55296, r[i] = 1023 & u | 56320;
                    break;
                default:
                    throw new Error("Bad UTF-8 encoding 0x" + a.toString(16));
            }
            if (i >= 65534) {
                var c = i + 1;
                r.length = c, n[n.length] = String.fromCharCode.apply(String, r), e -= c, i = -1;
            }
        }
        return i > 0 && (r.length = i, n[n.length] = String.fromCharCode.apply(String, r)), n.join("");
    }
    function f(t) {
        if (/^[\x00-\x7f]*$/.test(t) || !/^[\x00-\xff]*$/.test(t))
            return t;
        var e = t.length;
        return 0 === e ? "" : 1e5 > e ? c(t, e) : d(t, e);
    }
    function l(t, r) {
        return void 0 === t || null === t || 0 === t.length ? t : (t = u(t), r = u(r), e(s(n(t, !0), o(n(r, !1))), !1));
    }
    function h(e, n) {
        return t.btoa(l(e, n));
    }
    function m(t, r) {
        return void 0 === t || null === t || 0 === t.length ? t : (r = u(r), f(e(a(n(t, !1), o(n(r, !1))), !0)));
    }
    function _(e, n) {
        return void 0 === e || null === e || 0 === e.length ? e : m(t.atob(e), n);
    }
    "undefined" == typeof t.btoa && (t.btoa = function () {
        var t = "ABCDEFGHIJKLMNOPQRSTUVWXYZabcdefghijklmnopqrstuvwxyz0123456789+/".split("");
        return function (e) {
            var n, r, i, o, s, a, u;
            for (r = i = 0, o = e.length, s = o % 3, o -= s, a = o / 3 << 2, s > 0 && (a += 4), n = new Array(a) ; o > r;)
                u = e.charCodeAt(r++) << 16 | e.charCodeAt(r++) << 8 | e.charCodeAt(r++), n[i++] = t[u >> 18] + t[u >> 12 & 63] + t[u >> 6 & 63] + t[63 & u];
            return 1 == s ? (u = e.charCodeAt(r++), n[i++] = t[u >> 2] + t[(3 & u) << 4] + "==") : 2 == s && (u = e.charCodeAt(r++) << 8 | e.charCodeAt(r++), n[i++] = t[u >> 10] + t[u >> 4 & 63] + t[(15 & u) << 2] + "="), n.join("");
        };
    }()), "undefined" == typeof t.atob && (t.atob = function () {
        var t = [-1, -1, -1, -1, -1, -1, -1, -1, -1, -1, -1, -1, -1, -1, -1, -1, -1, -1, -1, -1, -1, -1, -1, -1, -1, -1, -1, -1, -1, -1, -1, -1, -1, -1, -1, -1, -1, -1, -1, -1, -1, -1, -1, 62, -1, -1, -1, 63, 52, 53, 54, 55, 56, 57, 58, 59, 60, 61, -1, -1, -1, -1, -1, -1, -1, 0, 1, 2, 3, 4, 5, 6, 7, 8, 9, 10, 11, 12, 13, 14, 15, 16, 17, 18, 19, 20, 21, 22, 23, 24, 25, -1, -1, -1, -1, -1, -1, 26, 27, 28, 29, 30, 31, 32, 33, 34, 35, 36, 37, 38, 39, 40, 41, 42, 43, 44, 45, 46, 47, 48, 49, 50, 51, -1, -1, -1, -1, -1];
        return function (e) {
            var n, r, i, o, s, a, u, c, d, f;
            if (u = e.length, u % 4 != 0)
                return "";
            if (/[^ABCDEFGHIJKLMNOPQRSTUVWXYZabcdefghijklmnopqrstuvwxyz0123456789\+\/\=]/.test(e))
                return "";
            for (c = "=" == e.charAt(u - 2) ? 1 : "=" == e.charAt(u - 1) ? 2 : 0, d = u, c > 0 && (d -= 4), d = 3 * (d >> 2) + c, f = new Array(d), s = a = 0; u > s && (n = t[e.charCodeAt(s++)], -1 != n) && (r = t[e.charCodeAt(s++)], -1 != r) && (f[a++] = String.fromCharCode(n << 2 | (48 & r) >> 4), i = t[e.charCodeAt(s++)], -1 != i) && (f[a++] = String.fromCharCode((15 & r) << 4 | (60 & i) >> 2), o = t[e.charCodeAt(s++)], -1 != o) ;)
                f[a++] = String.fromCharCode((3 & i) << 6 | o);
            return f.join("");
        };
    }());
    var y = 2654435769;
    t.XXTEA = { utf8Encode: u, utf8Decode: f, encrypt: l, encryptToBase64: h, decrypt: m, decryptFromBase64: _ };
}(window);

function decryptFromBase64(text, key) {
    return window.XXTEA.decryptFromBase64(text, key);
}

function encryptToBase64(text, key) {
    return window.XXTEA.encryptToBase64(text, key);
}