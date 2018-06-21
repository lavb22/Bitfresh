$target = $args[0]

" "
"Calculating hash for ""$target"""

$md5 = [System.BitConverter]::ToString([System.Security.Cryptography.MD5]::Create().ComputeHash([System.IO.File]::ReadAllBytes($target))).ToLowerInvariant().Replace('-', '')
$sha256 = [System.BitConverter]::ToString([System.Security.Cryptography.SHA256]::Create().ComputeHash([System.IO.File]::ReadAllBytes($target))).ToLowerInvariant().Replace('-', '')
$sha1 = [System.BitConverter]::ToString([System.Security.Cryptography.SHA1]::Create().ComputeHash([System.IO.File]::ReadAllBytes($target))).ToLowerInvariant().Replace('-', '')

#OUTPUT

" "
"----------------------------------------"
"Checksums:"
"----------------------------------------"
""
"MD5: $md5"
""
"SHA1: $sha1"
""
"SHA256: $sha256"
""
"----------------------------------------"
" "