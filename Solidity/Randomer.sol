pragma solidity ^0.8.6;

contract Rand {
    uint256 private seed;
    uint256 private next;

    uint256 private mod;
    uint256 private mult;
    uint256 private add;

    constructor(uint256 _seed) {
        seed = _seed;
        next = seed;
        mod = 2**256 - 1;
        mult = 75;
        add = 74;
    }

    function random() internal returns (uint256) {
        next = (mult * next + add) % mod;
        return next;
    }
}